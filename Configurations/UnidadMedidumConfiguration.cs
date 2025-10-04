using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class UnidadMedidumConfiguration : IEntityTypeConfiguration<UnidadMedidum>
{
    public void Configure(EntityTypeBuilder<UnidadMedidum> builder)
    {
        builder.ToTable("unidad_medida", "productos");
        builder.HasKey(x => x.UnidadMedidaId);
        builder.Property(x => x.UnidadMedidaId).HasColumnName("unidad_medida_id");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Abreviatura).HasColumnName("abreviatura").HasMaxLength(10).IsRequired();
        builder.Property(x => x.Descripcion).HasColumnName("descripcion");
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
        builder.Property(x => x.TipoUnidadMedidaId).HasColumnName("tipo_unidad_medida_id");

        builder.HasMany(u => u.LgConversionUnidadUnidadOrigens)
               .WithOne(c => c.UnidadOrigen)
               .HasForeignKey(c => c.UnidadOrigenId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_conv_unidad_origen");

        builder.HasMany(u => u.LgConversionUnidadUnidadDestinos)
               .WithOne(c => c.UnidadDestino)
               .HasForeignKey(c => c.UnidadDestinoId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_conv_unidad_destino");
    }
}

