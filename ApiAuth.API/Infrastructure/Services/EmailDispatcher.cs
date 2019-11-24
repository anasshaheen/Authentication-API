using ApiAuth.API.Data;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Options;
using ApiAuth.API.Models;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Services
{
    public class EmailDispatcher : IEmailDispatcher
    {
        private readonly ILogger<EmailDispatcher> _logger;
        private readonly DataContext _context;
        private readonly EmailSettings _emailSettings;

        public EmailDispatcher(
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailDispatcher> logger,
            DataContext context
            )
        {
            _logger = logger;
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        [AutomaticRetry(Attempts = 20, DelaysInSeconds = new[] { 60 })]
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                _logger.LogInformation("Start preparation for sending email!");

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                var message = new MimeMessage
                {
                    Subject = subject,
                    Body = bodyBuilder.ToMessageBody()
                };
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                message.To.Add(new MailboxAddress(email));

                using (var client = new SmtpClient())
                {
                    client.Connect(_emailSettings.MailServer, _emailSettings.MailPort);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SaveEmail(Email email)
        {
            try
            {
                _logger.LogInformation("Saving and dispatching email!");

                await _context.Emails.AddAsync(email);
                await _context.SaveChangesAsync();

                BackgroundJob.Enqueue(() => SendEmailAsync(email.To, email.Subject, email.Body));

                _logger.LogInformation("Email saved and dispatched.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
