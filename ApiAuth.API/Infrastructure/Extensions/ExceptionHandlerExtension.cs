using ApiAuth.API.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
