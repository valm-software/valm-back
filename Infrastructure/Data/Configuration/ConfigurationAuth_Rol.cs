using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationAuth_Rol : IEntityTypeConfiguration<Auth_Rol>
    {
        public void Configure(EntityTypeBuilder<Auth_Rol> builder)
        {
            builder.ToTable("Auth_Roles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Nombre).IsRequired().HasMaxLength(255);
        }
    }
}
