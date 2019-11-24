using ApiAuth.API.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Utils
{
    public static class DatabaseInitializer
    {
        public static async Task Seed(this RoleManager<IdentityRole> manager)
        {
            try
            {
                if (await manager.Roles.AnyAsync())
                {
                    return;
                }

                await manager.CreateAsync(new IdentityRole(Roles.Contributor));
                await manager.CreateAsync(new IdentityRole(Roles.User));
            }
            catch (Exception)
            {
                // do nothing
            }
        }
    }
}
