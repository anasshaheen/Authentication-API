using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Constants;
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
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly SignInManager<Models.AppUser> _signInManager;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly IEmailTemplateGenerator _emailTemplateGenerator;
        private readonly IEmailDispatcher _emailDispatcher;

        public RegisterUserHandler(
            UserManager<Models.AppUser> userManager,
            SignInManager<Models.AppUser> signInManager,
            ILogger<RegisterUserHandler> logger,
            IEmailTemplateGenerator emailTemplateGenerator,
            IEmailDispatcher emailDispatcher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailTemplateGenerator = emailTemplateGenerator;
            _emailDispatcher = emailDispatcher;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(RegisterUserHandler)}");

                var user = new Models.AppUser
                {
                    UserName = request.Input.Email,
                    Email = request.Input.Email,
                    Name = request.Input.Name,
                };

                var result = await _userManager.CreateAsync(user, request.Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, Roles.User);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = request.RequestUrlSchema +
                                      "://localhost:4200" +
                                      "/account/confirmEmail" +
                                      $"?code={code}&userId={user.Id}";
                    var title = "Confirm your email";
                    var body = new List<string>
                    {
                        "Please confirm your account by clicking the link below."
                    };
                    var url = new UrlEmailTemplateViewModel("Confirm account", callbackUrl);
                    var template = await _emailTemplateGenerator.RenderActionTemplate(title, body, url);

                    await _emailDispatcher.SaveEmail(new Email
                    {
                        Body = template,
                        Subject = title,
                        To = request.Input.Email
                    });
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }

                _logger.LogInformation($"Finished {nameof(RegisterUserHandler)}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
