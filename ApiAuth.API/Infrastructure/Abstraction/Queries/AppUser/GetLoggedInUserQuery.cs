using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Queries.AppUser
{
    public class GetLoggedInUserQuery : IRequest<AppUserViewModel>
    {
        public GetLoggedInUserQuery(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
