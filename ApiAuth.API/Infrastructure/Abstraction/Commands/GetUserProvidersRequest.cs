using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class GetUserProvidersRequest : IRequest<UserProviderViewModel>
    {
        public GetUserProvidersRequest(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
