using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class TipoUsoProductoConfiguration : IEntityTypeConfiguration<TipoUsoProducto>
{
    public void Configure(EntityTypeBuilder<TipoUsoProducto> builder)
    {
        // ===== Tabla =====
        builder.ToTable("tipo_uso_producto", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.TipoUsoProductoId);

        builder.Property(x => x.TipoUsoProductoId)
               .HasColumnName("tipo_uso_producto_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               // Si deseas limitar tamaño, agrega .HasMaxLength(##)
               .IsRequired();

        // ===== Índices =====
        builder.HasIndex(x => x.Nombre)
               .IsUnique()
               .HasDatabaseName("lg_tipo_uso_producto_nombre_key");
    }
}
