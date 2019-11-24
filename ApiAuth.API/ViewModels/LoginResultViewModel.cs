using Newtonsoft.Json;

namespace ApiAuth.API.ViewModels
{
    public class LoginResultViewModel
    {
        public LoginResultViewModel(AccessTokenViewModel accessToken, AppUserViewModel user)
        {
            AccessToken = accessToken;
            User = user;
        }

        [JsonProperty("access_token")]
        public AccessTokenViewModel AccessToken { get; }
        public AppUserViewModel User { get; }
    }

    public class AccessTokenViewModel
    {
        public AccessTokenViewModel(string token, double expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }

        public string Token { get; }
        [JsonProperty("expires_in")]
        public double ExpiresIn { get; }
    }
}
