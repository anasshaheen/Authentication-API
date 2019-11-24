using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Constants;
using ApiAuth.API.Infrastructure.Options;
using ApiAuth.API.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class FacebookExternalLoginHandler : IRequestHandler<FacebookExternalLoginRequest, LoginResultViewModel>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly FacebookAuthSettings _fbAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMapper _mapper;

        public FacebookExternalLoginHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager,
            IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
            HttpClient httpClient,
            IJwtFactory jwtFactory,
            IMapper mapper
            )
        {
            _logger = logger;
            _userManager = userManager;
            _httpClient = httpClient;
            _fbAuthSettings = fbAuthSettingsAccessor.Value;
            _jwtFactory = jwtFactory;
            _mapper = mapper;
        }

        public async Task<LoginResultViewModel> Handle(FacebookExternalLoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(FacebookExternalLoginHandler)}");

                var appAccessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
                var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

                var userAccessTokenValidationResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AccessToken}&access_token={appAccessToken.AccessToken}");
                var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

                if (!userAccessTokenValidation.Data.IsValid)
                {
                    throw new ArgumentException("Invalid facebook token.");
                }

                var userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name&access_token={request.AccessToken}");
                var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new Models.AppUser
                    {
                        Name = $"{userInfo.FirstName} {userInfo.LastName}",
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new ArgumentException(string.Join(",", result.Errors));
                    }

                    var externalLoginInfo = new ExternalLoginInfo(
                        null,
                        ExternalLoginProviders.Facebook,
                        $"{userInfo.Id}",
                        userInfo.Name
                    );
                    await _userManager.AddLoginAsync(user, externalLoginInfo);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = await _jwtFactory.GenerateEncodedToken(user.UserName, roles);

                _logger.LogInformation($"Finished {nameof(FacebookExternalLoginHandler)}");

                return new LoginResultViewModel(accessToken, _mapper.Map<AppUserViewModel>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
