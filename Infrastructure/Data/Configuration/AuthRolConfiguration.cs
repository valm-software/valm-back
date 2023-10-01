using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthRolConfiguration : IEntityTypeConfiguration<AuthRol>
    {
        public void Configure(EntityTypeBuilder<AuthRol> builder)
        {
            builder.ToTable("AuthRoles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Nombre).IsRequired().HasMaxLength(255);
        }
    }
}
