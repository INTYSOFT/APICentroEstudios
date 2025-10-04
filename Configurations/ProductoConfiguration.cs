using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        // Tabla y clave
        builder.ToTable("producto", "productos");
        builder.HasKey(p => p.ProductoId);
        builder.Property(p => p.ProductoId).HasColumnName("producto_id");

        // Columnas
        builder.Property(p => p.TipoProductoId).HasColumnName("tipo_producto_id");
        builder.Property(p => p.CategoriaId).HasColumnName("categoria_id");
        builder.Property(p => p.PlanContableId).HasColumnName("plan_contable_id");

        builder.Property(p => p.CodigoSunat)
               .HasColumnName("codigo_sunat")
               .HasMaxLength(16);

        builder.Property(p => p.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(p => p.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(p => p.Modelo)
               .HasColumnName("modelo")
               .HasMaxLength(64);

        builder.Property(p => p.MarcaId).HasColumnName("marca_id");
        builder.Property(p => p.ListaPrecioId).HasColumnName("lista_precio_id");
        builder.Property(p => p.Activo).HasColumnName("activo");
        builder.Property(p => p.ModeloId).HasColumnName("modelo_id");
        builder.Property(p => p.IsComprable).HasColumnName("is_comprable");
        builder.Property(p => p.IsVendible).HasColumnName("is_vendible");
        builder.Property(p => p.IsGasto).HasColumnName("is_gasto");

        // Índices útiles
        builder.HasIndex(p => p.CategoriaId)
               .HasDatabaseName("IX_producto_categoria");

        builder.HasIndex(p => new { p.Activo, p.Nombre })
               .HasDatabaseName("IX_producto_activo_nombre");

        // (Opcional) Unicidad de nombre por categoría
        // builder.HasIndex(p => new { p.CategoriaId, p.Nombre })
        //        .IsUnique()
        //        .HasDatabaseName("UX_producto_categoria_nombre");

        // Relación N:1 con Categorias
        builder.HasOne(p => p.Categoria)
               .WithMany(c => c.Productos)
               .HasForeignKey(p => p.CategoriaId)
               .OnDelete(DeleteBehavior.SetNull); // coherente con FK nullable
    }
}
