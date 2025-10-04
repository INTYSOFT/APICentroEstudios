using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoFichaTecnicaConfiguration : IEntityTypeConfiguration<ProductoFichaTecnica>
{
    public void Configure(EntityTypeBuilder<ProductoFichaTecnica> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("producto_ficha_tecnica", "productos");

        builder.HasKey(x => x.ProductoFichaTecnicaId);
        builder.Property(x => x.ProductoFichaTecnicaId)
               .HasColumnName("producto_ficha_tecnica_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoId)
               .HasColumnName("producto_id")
               .IsRequired();

        builder.Property(x => x.FichaTecnicaId)
               .HasColumnName("ficha_tecnica_id");

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .IsRequired();

        builder.Property(x => x.CategoriaFichaTecnicaDetalleId)
               .HasColumnName("categoria_ficha_tecnica_detalle_id");

        builder.Property(x => x.CategoriaFichaTecnicaId)
               .HasColumnName("categoria_ficha_tecnica_id");

        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(128);

        builder.Property(x => x.ListaFichaTecnicaId)
               .HasColumnName("lista_ficha_tecnica_id");

        // ===== Índices recomendados =====
        builder.HasIndex(x => x.ProductoId)
               .HasDatabaseName("IX_pft_producto");

        builder.HasIndex(x => new { x.ProductoId, x.Nombre })
               .HasDatabaseName("IX_pft_producto_nombre");

        builder.HasIndex(x => x.FichaTecnicaId)
               .HasDatabaseName("IX_pft_ficha_tecnica");

        builder.HasIndex(x => x.CategoriaFichaTecnicaId)
               .HasDatabaseName("IX_pft_categoria_ft");

        builder.HasIndex(x => x.CategoriaFichaTecnicaDetalleId)
               .HasDatabaseName("IX_pft_categoria_ft_detalle");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_pft_activo");

        // (Opcional) Evitar duplicar el mismo "Nombre" dentro del mismo Producto
        // builder.HasIndex(x => new { x.ProductoId, x.Nombre })
        //        .IsUnique()
        //        .HasDatabaseName("UX_pft_producto_nombre");

        // ===== Relaciones =====

        // PFT -> Producto (N:1, requerido)
        // Sugerencia: Restrict evita borrar accidentalmente fichas al eliminar un producto.
        // Si deseas borrar en cascada las fichas técnicas al borrar el producto, cambia a Cascade.
        builder.HasOne(x => x.Producto)
               .WithMany(p => p.ProductoFichaTecnicas)
               .HasForeignKey(x => x.ProductoId)
               .OnDelete(DeleteBehavior.Restrict);

        // PFT -> FichaTecnica (N:1, opcional)
        builder.HasOne(x => x.FichaTecnica)
               .WithMany() // usa .WithMany(ft => ft.ProductoFichaTecnicas) si existe esa navegación
               .HasForeignKey(x => x.FichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        // PFT -> CategoriaFichaTecnicaDetalle (N:1, opcional)
        builder.HasOne(x => x.CategoriaFichaTecnicaDetalle)
               .WithMany() // usa .WithMany(d => d.ProductoFichaTecnicas) si existe
               .HasForeignKey(x => x.CategoriaFichaTecnicaDetalleId)
               .OnDelete(DeleteBehavior.SetNull);

        // PFT -> CategoriaFichaTecnica (N:1, opcional)
        builder.HasOne(x => x.CategoriaFichaTecnica)
               .WithMany() // usa .WithMany(c => c.ProductoFichaTecnicas) si existe
               .HasForeignKey(x => x.CategoriaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        // PFT (1) -> ProductoFichaTecnicaDetalle (N)
        // Si la entidad detalle tiene FK ProductoFichaTecnicaId y navegación .ProductoFichaTecnica
        // esto aplica borrado en cascada de los DETALLES al borrar una PFT (no el Producto).
        builder.HasMany(x => x.ProductoFichaTecnicaDetalles)
               .WithOne(d => d.ProductoFichaTecnica)
               .HasForeignKey(d => d.ProductoFichaTecnicaId)
               .OnDelete(DeleteBehavior.Cascade);        

    }
}
