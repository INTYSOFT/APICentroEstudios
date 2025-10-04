using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ModeloConfiguration : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        // ===== Tabla =====
        builder.ToTable("modelo", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ModeloId);
        builder.Property(x => x.ModeloId)
               .HasColumnName("modelo_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(256)
               .IsRequired();

        builder.Property(x => x.CodigoModelo)
               .HasColumnName("codigo_modelo")
               .HasMaxLength(50);

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)  // si deseas que por defecto sea activo
               .IsRequired();

        // ===== Índices =====
        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_modelo_nombre");

        builder.HasIndex(x => x.CodigoModelo)
               .HasDatabaseName("IX_modelo_codigo");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_modelo_activo");

        // (Opcional) Si el código de modelo debe ser único:
        // builder.HasIndex(x => x.CodigoModelo)
        //        .IsUnique()
        //        .HasDatabaseName("UX_modelo_codigo");

        // (Opcional) Check constraints (elige sintaxis según proveedor)
        // SQL Server:
        // builder.ToTable("modelo", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_modelo_nombre_len", "LEN([nombre]) BETWEEN 1 AND 256");
        //     tb.HasCheckConstraint("CK_modelo_codigo_len", "([codigo_modelo] IS NULL) OR (LEN([codigo_modelo]) BETWEEN 1 AND 50)");
        // });
        //
        // PostgreSQL:
        // builder.ToTable("modelo", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_modelo_nombre_len", "char_length(\"nombre\") BETWEEN 1 AND 256");
        //     tb.HasCheckConstraint("CK_modelo_codigo_len", "(\"codigo_modelo\" IS NULL) OR (char_length(\"codigo_modelo\") BETWEEN 1 AND 50)");
        // });
    }
}
