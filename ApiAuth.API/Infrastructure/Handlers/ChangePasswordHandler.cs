using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, IdentityResult>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public ChangePasswordHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(ChangePasswordHandler)}");

                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with UserName '{request.UserName}'.");
                }

                var result = await _userManager.ChangePasswordAsync(
                    user,
                    request.Input.OldPassword,
                    request.Input.NewPassword
                );

                _logger.LogInformation($"Finished {nameof(ChangePasswordHandler)}");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
