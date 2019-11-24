using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class MiddlewarePipelineExtensions
    {
        public static IApplicationBuilder UseComponents(this IApplicationBuilder app)
        {
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(opts => opts.AllowAnyHeader()
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc();

            return app;
        }

        public static IApplicationBuilder UseDiSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "GP Taxi API");
            });

            return app;
        }

        public static IApplicationBuilder UseHangFire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            return app;
        }
    }
}