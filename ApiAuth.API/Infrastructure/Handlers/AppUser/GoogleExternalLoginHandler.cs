using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
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
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Handlers.AppUser
{
    public class GoogleExternalLoginHandler : IRequestHandler<GoogleExternalLoginCommand, LoginResultViewModel>
    {
        private readonly ILogger<GetUserProvidersHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMapper _mapper;

        public GoogleExternalLoginHandler(
            ILogger<GetUserProvidersHandler> logger,
            UserManager<Models.AppUser> userManager,
            IOptions<GoogleAuthSettings> fbAuthSettingsAccessor,
            HttpClient httpClient,
            IJwtFactory jwtFactory,
            IMapper mapper
            )
        {
            _logger = logger;
            _userManager = userManager;
            _httpClient = httpClient;
            _googleAuthSettings = fbAuthSettingsAccessor.Value;
            _jwtFactory = jwtFactory;
            _mapper = mapper;
        }

        public async Task<LoginResultViewModel> Handle(GoogleExternalLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(GoogleExternalLoginHandler)}");

                var url = $"https://www.googleapis.com/oauth2/v4/token?code={request.Code}&client_id={_googleAuthSettings.ClientId}&client_secret={_googleAuthSettings.ClientSecret}&redirect_uri=http://localhost:5000/google-auth.html&grant_type=authorization_code";
                var uri = new Uri(url);
                _httpClient.BaseAddress = uri;
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var req = new HttpRequestMessage(HttpMethod.Post, uri);
                var httpResponseMessage = await _httpClient.SendAsync(req, cancellationToken);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new ArgumentException("Invalid google token.");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
                var httpContent = httpResponseMessage.Content;
                var responseString = await httpContent.ReadAsStringAsync();
                var resultData = JsonConvert.DeserializeObject<GoogleAppAccessToken>(responseString);

                var userAccessTokenValidationResponse = await _httpClient.GetStringAsync($"https://www.googleapis.com/plus/v1/people/me/openIdConnect?access_token={resultData.AccessToken}");
                var userInfo = JsonConvert.DeserializeObject<GoogleUserData>(userAccessTokenValidationResponse);

                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new Models.AppUser
                    {
                        Name = $"{userInfo.GivenName} {userInfo.FamilyName}",
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
                        ExternalLoginProviders.Google,
                        userInfo.Sub,
                        userInfo.Name
                    );
                    await _userManager.AddLoginAsync(user, externalLoginInfo);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = await _jwtFactory.GenerateEncodedToken(user.UserName, roles);

                _logger.LogInformation($"Finished {nameof(GoogleExternalLoginHandler)}");

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
