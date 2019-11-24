using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class GetLoggedInUserRequest : IRequest<AppUserViewModel>
    {
        public GetLoggedInUserRequest(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
