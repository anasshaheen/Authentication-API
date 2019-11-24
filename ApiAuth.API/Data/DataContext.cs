using ApiAuth.API.Infrastructure.Extensions;
using ApiAuth.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiAuth.API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurations();
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}
