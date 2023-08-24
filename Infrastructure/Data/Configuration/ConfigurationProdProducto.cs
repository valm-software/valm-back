using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationProdProducto : IEntityTypeConfiguration<ProdProducto>
    {
        public void Configure(EntityTypeBuilder<ProdProducto> builder)
        {
            builder.ToTable("ProdProductos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Descripcion)
                .HasMaxLength(250);

            builder.Property(p => p.ValorCosto)
                .IsRequired()
                .HasColumnType("decimal(18,2)");                

            builder.Property(p => p.ValorVenta)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ValorInicial)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ValorCuota)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.NumCuotas)
                .IsRequired()
                .HasColumnType("int");

            //builder.Property(p => p.ConfActivo)
            //    .IsRequired()
            //    .HasColumnType("tinyint(1)");
            builder.Property(p => p.ConfActivo)
                .IsRequired()
                .HasColumnType("tinyint(1)");

            builder.Property(p => p.ConfPorUtilidad)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP)");

        }
    }
}
