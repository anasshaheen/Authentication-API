using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class ConfirmEmailHandler : INotificationHandler<ConfirmEmailEvent>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public ConfirmEmailHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task Handle(ConfirmEmailEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(ConfirmEmailHandler)}");

                if (string.IsNullOrWhiteSpace(notification.UserId) ||
                    string.IsNullOrWhiteSpace(notification.Code))
                {
                    throw new ArgumentException();
                }

                var user = await _userManager.FindByIdAsync($"{notification.UserId}");
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with ID '{notification.UserId}'.");
                }

                var result = await _userManager.ConfirmEmailAsync(user, notification.Code);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Error confirming email for user with ID '{notification.UserId}':");
                }

                _logger.LogInformation($"Finished {nameof(ConfirmEmailHandler)}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
