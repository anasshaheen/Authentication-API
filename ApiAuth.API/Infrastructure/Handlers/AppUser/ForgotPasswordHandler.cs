using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
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

namespace ApiAuth.API.Infrastructure.Handlers.AppUser
{
    public class ForgotPasswordHandler : INotificationHandler<ForgotPasswordEvent>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly IEmailTemplateGenerator _emailTemplateGenerator;

        public ForgotPasswordHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager,
            IEmailDispatcher emailDispatcher,
            IEmailTemplateGenerator emailTemplateGenerator
            )
        {
            _logger = logger;
            _userManager = userManager;
            _emailDispatcher = emailDispatcher;
            _emailTemplateGenerator = emailTemplateGenerator;
        }

        public async Task Handle(ForgotPasswordEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(ForgotPasswordHandler)}");

                var user = await _userManager.FindByEmailAsync(notification.Input.Email);
                if (user == null)
                {
                    throw new NullReferenceException();
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    throw new InvalidOperationException("Please, Confirm your email first then try.");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = notification.RequestSchema +
                                  "://localhost:4200" +
                                  "/account/resetPassword" +
                                  $"?code={code}";

                var title = "Reset Password";
                var body = new List<string>
                {
                    "Please reset your password by clicking the link below."
                };
                var url = new UrlEmailTemplateViewModel("Reset your account", callbackUrl);
                var template = await _emailTemplateGenerator.RenderActionTemplate(title, body, url);

                await _emailDispatcher.SaveEmail(new Email
                {
                    To = notification.Input.Email,
                    Subject = title,
                    Body = template
                });

                _logger.LogInformation($"Finished {nameof(ForgotPasswordHandler)}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
