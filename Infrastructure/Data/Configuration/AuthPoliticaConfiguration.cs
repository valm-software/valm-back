using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthPoliticaConfiguration : IEntityTypeConfiguration<AuthPolitica>
    {
        public void Configure(EntityTypeBuilder<AuthPolitica> builder)
        {
            builder.ToTable("AuthPoliticas");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.NombrePolitica).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Modulo).IsRequired().HasMaxLength(255);
        }
    }
}
