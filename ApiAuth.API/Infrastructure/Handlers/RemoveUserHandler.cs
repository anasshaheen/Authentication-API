using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class RemoveUserHandler : INotificationHandler<RemoveUserEvent>
    {
        private readonly ILogger<RemoveUserHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public RemoveUserHandler(
            ILogger<RemoveUserHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task Handle(RemoveUserEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(RemoveUserHandler)}");

                var user = await _userManager.FindByNameAsync(notification.UserName);
                if (user == null)
                {
                    throw new NullReferenceException();
                }

                await _userManager.DeleteAsync(user);

                _logger.LogInformation($"Finished {nameof(RemoveUserHandler)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}