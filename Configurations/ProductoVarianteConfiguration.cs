using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoVarianteConfiguration : IEntityTypeConfiguration<ProductoVariante>
{
    public void Configure(EntityTypeBuilder<ProductoVariante> builder)
    {
        // ===== Tabla =====
        builder.ToTable("producto_variante", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ProductoVarianteId);
        builder.Property(x => x.ProductoVarianteId)
               .HasColumnName("producto_variante_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoId)
               .HasColumnName("producto_id")
               .IsRequired();

        builder.Property(x => x.ListaFichaTecnicaId)
               .HasColumnName("lista_ficha_tecnica_id");

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.GestionarPrecio)
               .HasColumnName("gestionar_precio");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true);

        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(128);

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ProductoId)
               .HasDatabaseName("IX_pv_producto");

        builder.HasIndex(x => x.ListaFichaTecnicaId)
               .HasDatabaseName("IX_pv_lista_ft");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_pv_activo");

        // (Opcional) Evitar duplicar nombre de variante por producto
        // builder.HasIndex(x => new { x.ProductoId, x.Nombre })
        //        .IsUnique()
        //        .HasDatabaseName("UX_pv_producto_nombre");

        // ===== Relaciones =====

        // (N) ProductoVariante -> (1) Producto (requerida)
        // Si agregas en Producto: ICollection<ProductoVariante> ProductoVariantes, cambia .WithMany() por .WithMany(p => p.ProductoVariantes)
        builder.HasOne(x => x.Producto)
               .WithMany()
               .HasForeignKey(x => x.ProductoId)
               .OnDelete(DeleteBehavior.Cascade);        

        // 1:N ProductoVariante -> ProductoVarianteDetalle
        // Requiere que ProductoVarianteDetalle tenga FK ProductoVarianteId y navegación .ProductoVariante
        builder.HasMany(x => x.VarianteDetalles)
               .WithOne(d => d.ProductoVariante)
               .HasForeignKey(d => d.ProductoVarianteId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
