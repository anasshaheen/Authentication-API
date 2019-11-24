using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Queries.AppUser
{
    public class GetUserProvidersQuery : IRequest<UserProviderViewModel>
    {
        public GetUserProvidersQuery(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
