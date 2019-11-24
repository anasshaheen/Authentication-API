using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class DeleteProviderHandler : INotificationHandler<DeleteProviderEvent>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public DeleteProviderHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task Handle(DeleteProviderEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(DeleteProviderHandler)}");

                var user = await _userManager.FindByNameAsync(notification.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with UserName '{notification.UserName}'.");
                }

                var result = await _userManager.RemoveLoginAsync(user, notification.LoginProvider, notification.ProviderKey);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
                }

                _logger.LogInformation($"Finished {nameof(DeleteProviderHandler)}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
