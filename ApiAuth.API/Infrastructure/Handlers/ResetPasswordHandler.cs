using System;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, IdentityResult>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;

        public ResetPasswordHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(ResetPasswordHandler)}");

                var user = await _userManager.FindByEmailAsync(request.Input.Email);
                if (user == null)
                {
                    throw new ArgumentException();
                }

                var result = await _userManager.ResetPasswordAsync(user, request.Input.Code, request.Input.Password);

                _logger.LogInformation($"Finished {nameof(ResetPasswordHandler)}");

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
