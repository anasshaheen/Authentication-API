using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class GoogleExternalLoginCommand : IRequest<LoginResultViewModel>
    {
        public GoogleExternalLoginCommand(string accessToken, string code)
        {
            AccessToken = accessToken;
            Code = code;
        }

        public string AccessToken { get; }
        public string Code { get; }
    }
}