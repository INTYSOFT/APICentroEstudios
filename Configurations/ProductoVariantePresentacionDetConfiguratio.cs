using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoVariantePresentacionDetConfiguration
    : IEntityTypeConfiguration<ProductoVariantePresentacionDet>
{
    public void Configure(EntityTypeBuilder<ProductoVariantePresentacionDet> builder)
    {
        // ===== Tabla (y opcionales constraints) =====
        builder.ToTable("producto_variante_presentacion_det", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ProductoVariantePresentacionDetId);

        builder.Property(x => x.ProductoVariantePresentacionDetId)
               .HasColumnName("producto_variante_presentacion_det_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoVariantePresentacionId)
               .HasColumnName("producto_variante_presentacion_id")
               .IsRequired();

        builder.Property(x => x.ProductoVarianteDetalleId)
               .HasColumnName("producto_variante_detalle_id")
               .IsRequired();

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true);

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ProductoVariantePresentacionId)
               .HasDatabaseName("IX_pvpd_presentacion");

        builder.HasIndex(x => x.ProductoVarianteDetalleId)
               .HasDatabaseName("IX_pvpd_detalle");

        // (Opcional) Evitar duplicados del mismo Detalle en la misma Presentación
        builder.HasIndex(x => new { x.ProductoVariantePresentacionId, x.ProductoVarianteDetalleId })
               .IsUnique()
               .HasDatabaseName("UX_pvpd_presentacion_detalle");

        // ===== Relaciones =====

        // (N) Det -> (1) Cabecera Presentación (requerida)
        builder.HasOne(x => x.ProductoVariantePresentacion)
               .WithMany(p => p.Detalles)
               .HasForeignKey(x => x.ProductoVariantePresentacionId)
               .OnDelete(DeleteBehavior.Cascade);

        // (N) Det -> (1) Variante Detalle (requerida)
        // Si en ProductoVarianteDetalle agregas la colección inversa, cambia .WithMany()
        // por .WithMany(d => d.ProductoVariantePresentacionDetalles)
        builder.HasOne(x => x.ProductoVarianteDetalle)
               .WithMany()
               .HasForeignKey(x => x.ProductoVarianteDetalleId)
               .OnDelete(DeleteBehavior.Restrict); // evita borrar el detalle si está referenciado por presentaciones
    }
}
