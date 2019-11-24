using MediatR;
using System.Collections.Generic;

namespace ApiAuth.API.Infrastructure.Abstraction.Queries.System
{
    public class GetExternalLoginProvidersQuery : IRequest<List<string>>
    {
    }
}
