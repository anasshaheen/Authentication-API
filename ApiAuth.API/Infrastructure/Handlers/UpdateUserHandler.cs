using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class UpdateUserHandler : INotificationHandler<UpdateUserEvent>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public UpdateUserHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task Handle(UpdateUserEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(UpdateUserHandler)}");

                var user = await _userManager.FindByNameAsync(notification.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with UserName '{notification.UserName}'.");
                }

                var email = await _userManager.GetEmailAsync(user);
                if (notification.Input.Email != email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, notification.Input.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                    }
                }

                user.Name = notification.Input.Name;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"Finished {nameof(UpdateUserHandler)}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
