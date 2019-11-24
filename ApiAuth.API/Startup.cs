using ApiAuth.API.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAuth.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataContext(Configuration);
            services.AddIdentityConfig();
            services.AddAuth(Configuration);
            services.AddOptions(Configuration);
            services.AddHangFireConfig(Configuration);
            services.AddServices();
            services.AddComponents();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseApiExceptionHandler();
                app.UseHsts();
            }

            app.UseDiSwagger();
            app.UseHangFire();
            app.UseComponents();
        }
    }
}
