using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class GoogleExternalLoginRequest : IRequest<LoginResultViewModel>
    {
        public GoogleExternalLoginRequest(string accessToken, string code)
        {
            AccessToken = accessToken;
            Code = code;
        }

        public string AccessToken { get; }
        public string Code { get; }
    }
}