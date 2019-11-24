using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using ApiAuth.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class GetUserProvidersHandler : IRequestHandler<GetUserProvidersRequest, UserProviderViewModel>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly SignInManager<Models.AppUser> _signInManager;

        public GetUserProvidersHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager,
            SignInManager<Models.AppUser> signInManager
        )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<UserProviderViewModel> Handle(GetUserProvidersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(GetUserProvidersHandler)}");

                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new NullReferenceException($"Unable to load user with UserName '{request.UserName}'.");
                }

                var userProviders = await _userManager.GetLoginsAsync(user);
                var remainingProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                    .Where(auth => userProviders.All(ul => auth.Name != ul.LoginProvider))
                    .ToList();

                _logger.LogInformation($"Finished {nameof(GetUserProvidersHandler)}");

                return new UserProviderViewModel
                {
                    UnusedLoginProviders = remainingProviders,
                    UsedLoginProvider = userProviders.ToList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
