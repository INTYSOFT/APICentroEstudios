using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class TipoProductosConfiguration : IEntityTypeConfiguration<TipoProductos>
{
    public void Configure(EntityTypeBuilder<TipoProductos> builder)
    {
        // ===== Tabla =====
        builder.ToTable("tipo_producto", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.TipoProductoId);
        builder.Property(x => x.TipoProductoId).HasColumnName("tipo_producto_id");

        // ===== Columnas =====
        builder.Property(x => x.TipoProducto)
               .HasColumnName("tipo_producto")
               .HasMaxLength(32)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(128);

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)  // si quieres que por defecto sea activo
               .IsRequired();

        // ===== Índices =====
        builder.HasIndex(x => x.TipoProducto)
               .IsUnique()
               .HasDatabaseName("lg_tipo_producto_tipo_producto_key");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_tipo_producto_activo");

        // ===== (Opcional) Check constraints según proveedor =====
        // SQL Server:
        // builder.ToTable("tipo_producto", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_tipo_prod_len",
        //         "LEN([tipo_producto]) BETWEEN 1 AND 32");
        //     tb.HasCheckConstraint("CK_tipo_prod_desc_len",
        //         "[descripcion] IS NULL OR LEN([descripcion]) <= 128");
        // });
        //
        // PostgreSQL:
        // builder.ToTable("tipo_producto", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_tipo_prod_len",
        //         "char_length(\"tipo_producto\") BETWEEN 1 AND 32");
        //     tb.HasCheckConstraint("CK_tipo_prod_desc_len",
        //         "\"descripcion\" IS NULL OR char_length(\"descripcion\") <= 128");
        // });
    }
}
