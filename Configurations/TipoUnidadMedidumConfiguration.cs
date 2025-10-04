using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class TipoUnidadMedidumConfiguration : IEntityTypeConfiguration<TipoUnidadMedidum>
{
    public void Configure(EntityTypeBuilder<TipoUnidadMedidum> builder)
    {
        // ===== Tabla =====
        builder.ToTable("tipo_unidad_medida", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.TipoUnidadMedidaId);
        builder.Property(x => x.TipoUnidadMedidaId)
               .HasColumnName("tipo_unidad_medida_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        // ===== Índices =====
        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_tipo_um_nombre");

        // (Opcional) Unicidad por nombre, si aplica a tu negocio:
        // builder.HasIndex(x => x.Nombre)
        //        .IsUnique()
        //        .HasDatabaseName("UX_tipo_um_nombre");

        // ===== Relaciones =====
        // TipoUnidadMedidum (1) -> (N) UnidadMedidum
        // Requiere que UnidadMedidum tenga FK TipoUnidadMedidaId
        builder.HasMany(x => x.LgUnidadMedida)
               .WithOne(u => u.TipoUnidadMedida)
               .HasForeignKey(u => u.TipoUnidadMedidaId)
               .OnDelete(DeleteBehavior.Restrict); // evita borrar tipo en uso
    }
}
