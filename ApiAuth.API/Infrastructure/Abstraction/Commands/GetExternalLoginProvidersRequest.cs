using System.Collections.Generic;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class GetExternalLoginProvidersRequest : IRequest<List<string>>
    {
    }
}
