using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoVarianteDetalleConfiguration : IEntityTypeConfiguration<ProductoVarianteDetalle>
{
    public void Configure(EntityTypeBuilder<ProductoVarianteDetalle> builder)
    {
        // ===== Tabla =====
        builder.ToTable("producto_variante_detalle", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ProductoVarianteDetalleId);

        builder.Property(x => x.ProductoVarianteDetalleId)
               .HasColumnName("producto_variante_detalle_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoVarianteId)
               .HasColumnName("producto_variante_id")
               .IsRequired();

        builder.Property(x => x.DetalleListaFichaTecnicaId)
               .HasColumnName("detalle_lista_ficha_tecnica_id");

        builder.Property(x => x.Dato)
               .HasColumnName("dato")
               .HasMaxLength(64);

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(512);

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true);

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ProductoVarianteId)
               .HasDatabaseName("IX_pvd_variante");

        builder.HasIndex(x => x.DetalleListaFichaTecnicaId)
               .HasDatabaseName("IX_pvd_detalle_lista");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_pvd_activo");

        // (Opcional) Evitar duplicados del mismo atributo dentro de la misma variante
        // builder.HasIndex(x => new { x.ProductoVarianteId, x.DetalleListaFichaTecnicaId, x.Dato })
        //        .IsUnique()
        //        .HasDatabaseName("UX_pvd_variante_detalle_dato");

        // ===== Relaciones =====

        // (N) Detalle -> (1) ProductoVariante (requerida)
        // Enlaza con ProductoVariante.VarianteDetalles
        builder.HasOne(x => x.ProductoVariante)
               .WithMany(v => v.VarianteDetalles)
               .HasForeignKey(x => x.ProductoVarianteId)
               .OnDelete(DeleteBehavior.Cascade);

        // (N) Detalle -> (1) DetalleListaFichaTecnica (opcional)
        // Si en DetalleListaFichaTecnica agregaste la colección LgProductoVarianteDetalles,
        // cámbialo a .WithMany(l => l.LgProductoVarianteDetalles)
        builder.HasOne(x => x.DetalleListaFichaTecnica)
               .WithMany() // .WithMany(l => l.LgProductoVarianteDetalles)
               .HasForeignKey(x => x.DetalleListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
