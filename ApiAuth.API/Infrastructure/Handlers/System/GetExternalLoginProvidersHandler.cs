using ApiAuth.API.Infrastructure.Abstraction.Queries.System;
using ApiAuth.API.Infrastructure.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Handlers.System
{
    public class GetExternalLoginProvidersHandler : IRequestHandler<GetExternalLoginProvidersQuery, List<string>>
    {
        private readonly ILogger<GetExternalLoginProvidersHandler> _logger;

        public GetExternalLoginProvidersHandler(
            ILogger<GetExternalLoginProvidersHandler> logger
        )
        {
            _logger = logger;
        }

        public Task<List<string>> Handle(GetExternalLoginProvidersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(GetExternalLoginProvidersHandler)}");

                var result = new List<string>
                {
                    ExternalLoginProviders.Google,
                    ExternalLoginProviders.Facebook
                };

                _logger.LogInformation($"Finished {nameof(GetExternalLoginProvidersHandler)}");

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
