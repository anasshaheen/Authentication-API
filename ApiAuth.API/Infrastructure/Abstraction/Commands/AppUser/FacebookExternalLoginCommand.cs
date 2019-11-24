using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class FacebookExternalLoginCommand : IRequest<LoginResultViewModel>
    {
        public FacebookExternalLoginCommand(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}
