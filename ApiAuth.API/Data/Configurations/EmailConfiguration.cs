using ApiAuth.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiAuth.API.Data.Configurations
{
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.Property(e => e.To).IsRequired();
            builder.Property(e => e.Subject).IsRequired();
            builder.Property(e => e.Body).IsRequired();
        }
    }
}