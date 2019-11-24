using ApiAuth.API.Infrastructure.Abstraction.Commands;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Models;
using ApiAuth.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class SendVerificationEmailHandler : INotificationHandler<SendVerificationEmailEvent>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailTemplateGenerator _emailTemplateGenerator;
        private readonly IEmailDispatcher _emailDispatcher;

        public SendVerificationEmailHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<AppUser> userManager,
            IEmailTemplateGenerator emailTemplateGenerator,
            IEmailDispatcher emailDispatcher
            )
        {
            _logger = logger;
            _userManager = userManager;
            _emailTemplateGenerator = emailTemplateGenerator;
            _emailDispatcher = emailDispatcher;
        }

        public async Task Handle(SendVerificationEmailEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(SendVerificationEmailHandler)}");

                var user = await _userManager.FindByNameAsync(notification.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with UserName '{notification.UserName}'.");
                }

                var email = await _userManager.GetEmailAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = notification.RequestSchema +
                                  "://localhost:4200" +
                                  "/account/confirmEmail" +
                                  $"?code={code}&userId={user.Id}";

                var title = "Confirm your email";
                var body = new List<string>
                {
                    "Please confirm your account by clicking the link below."
                };
                var template = await _emailTemplateGenerator.RenderActionTemplate(title, body, new UrlEmailTemplateViewModel("Confirm account", callbackUrl));

                await _emailDispatcher.SaveEmail(new Email
                {
                    To = email,
                    Subject = title,
                    Body = template
                });

                _logger.LogInformation($"Finished {nameof(SendVerificationEmailHandler)}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
