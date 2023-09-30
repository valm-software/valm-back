using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationAuth_UsuarioRol : IEntityTypeConfiguration<Auth_UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<Auth_UsuarioRol> builder)
        {
            builder.ToTable("Auth_UsuariosRoles");
            builder.HasKey(ur => new { ur.UsuarioId, ur.RolId });

            builder.HasOne(ur => ur.Auth_Usuario)
                .WithMany(u => u.Auth_UsuarioRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            builder.HasOne(ur => ur.Auth_Rol)
                .WithMany(r => r.Auth_UsuarioRoles)
                .HasForeignKey(ur => ur.RolId);
        }
    }
}
