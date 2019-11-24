using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Handlers.AppUser
{
    public class SetPasswordHandler : IRequestHandler<SetPasswordCommand, IdentityResult>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public SetPasswordHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
        )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(SetPasswordHandler)}");

                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with ID '{request.UserName}'.");
                }

                var result = await _userManager.AddPasswordAsync(user, request.Input.NewPassword);

                _logger.LogInformation($"Finished {nameof(SetPasswordHandler)}");

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
