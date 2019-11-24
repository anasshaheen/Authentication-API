using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiAuth.API.Infrastructure.Abstraction.Commands;
using ApiAuth.API.Infrastructure.Constants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApiAuth.API.Infrastructure.Handlers
{
    public class GetExternalLoginProvidersHandler : IRequestHandler<GetExternalLoginProvidersRequest, List<string>>
    {
        private readonly ILogger<GetExternalLoginProvidersHandler> _logger;

        public GetExternalLoginProvidersHandler(
            ILogger<GetExternalLoginProvidersHandler> logger
        )
        {
            _logger = logger;
        }

        public Task<List<string>> Handle(GetExternalLoginProvidersRequest request, CancellationToken cancellationToken)
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
