using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class JwtLoginHandler : IRequestHandler<JwtLoginCommand, LoginResultViewModel>
    {
        private readonly ILogger<JwtLoginHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMapper _mapper;

        public JwtLoginHandler(
            ILogger<JwtLoginHandler> logger,
            UserManager<Models.AppUser> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper
        )
        {
            _logger = logger;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _mapper = mapper;
        }

        public async Task<LoginResultViewModel> Handle(JwtLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(JwtLoginHandler)}");

                var user = await _userManager.FindByEmailAsync(request.Input.Email);
                if (user == null)
                {
                    throw new NullReferenceException("User not found!");
                }

                if (!request.ByPassPassword)
                {
                    var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Input.Password);
                    if (!isPasswordCorrect)
                    {
                        throw new UnauthorizedAccessException("Username or password are incorrect!");
                    }
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var userViewModel = _mapper.Map<AppUserViewModel>(user);
                userViewModel.Roles = userRoles.ToList();

                var accessToken = await _jwtFactory.GenerateEncodedToken(user.UserName, userRoles);

                _logger.LogInformation($"Finished {nameof(JwtLoginHandler)}");

                return new LoginResultViewModel(accessToken, userViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
