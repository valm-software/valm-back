using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class ConfigurationUsrRolPrivilegio : IEntityTypeConfiguration<UsrRolPrivilegio>
    {
        public void Configure(EntityTypeBuilder<UsrRolPrivilegio> builder)
        {
            builder.ToTable("UsrRolesPrivilegios");
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.UsrRol)
                    .WithMany(p => p.UsrRolPrivilegio)
                    .HasForeignKey(p => p.IdRol);

            builder.HasOne(p => p.UsrPrivilegio)
                    .WithMany(p => p.UsrRolPrivilegio)
                    .HasForeignKey(p => p.IdPrivilegio);
        }
    }
}
