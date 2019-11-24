using ApiAuth.API.Data;
using ApiAuth.API.Infrastructure.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost Migrate(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                using var vanHackContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                try
                {
                    vanHackContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return webHost;
        }

        public static IWebHost SeedDb(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                roleManager.Seed().Wait();
            }

            return webHost;
        }
    }
}
