using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class FacebookExternalLoginRequest : IRequest<LoginResultViewModel>
    {
        public FacebookExternalLoginRequest(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}
