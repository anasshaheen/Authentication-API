using ApiAuth.API.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class DbConfigurationExtension
    {
        public static void ApplyConfigurations(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new EmailConfiguration());
        }
    }
}
