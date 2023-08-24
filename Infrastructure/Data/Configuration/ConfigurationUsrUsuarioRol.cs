using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class ConfigurationUsrUsuarioRol : IEntityTypeConfiguration<UsrUsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsrUsuarioRol> builder)
        {
            builder.ToTable("UsrUsuariosRoles");

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.UsrUsuario)
                .WithMany(p => p.UsrUsuarioRol)
                .HasForeignKey(p => p.IdUsuario);

            builder.HasOne(p => p.UsrRol)
                .WithMany(p => p.UsrUsuarioRol)
                .HasForeignKey(p => p.IdRol);
        }
    }
}


//public int IdUsuarioPermiso { get; set; }
//public int IdPermiso { get; set; }
//public UsrPermiso UsrPermiso { get; set; }
//public int IdUsuario { get; set; }
//public UsrUsuario UsrUsuario { get; set; }