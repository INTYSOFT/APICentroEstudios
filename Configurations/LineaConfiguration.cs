using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class LineaConfiguration : IEntityTypeConfiguration<Linea>
{
    public void Configure(EntityTypeBuilder<Linea> builder)
    {
        // ===== Tabla =====
        builder.ToTable("lg_linea", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.LineaId);
        builder.Property(x => x.LineaId).HasColumnName("linea_id");

        // ===== Columnas =====
        builder.Property(x => x.Codigo)
               .HasColumnName("codigo")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true); // mantiene null permitido, default en inserción

        // ===== Índices =====
        builder.HasIndex(x => x.Codigo)
               .IsUnique()
               .HasDatabaseName("lg_linea_codigo_key"); // respeta el nombre que tenías

        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_lg_linea_nombre");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_lg_linea_activo");

        // ===== (Opcional) Check constraints =====
        // SQL Server:
        // builder.ToTable("lg_linea", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_lg_linea_codigo_len", "LEN([codigo]) BETWEEN 1 AND 50");
        //     tb.HasCheckConstraint("CK_lg_linea_nombre_len", "LEN([nombre]) BETWEEN 1 AND 100");
        // });
        //
        // PostgreSQL:
        // builder.ToTable("lg_linea", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_lg_linea_codigo_len", "char_length(\"codigo\") BETWEEN 1 AND 50");
        //     tb.HasCheckConstraint("CK_lg_linea_nombre_len", "char_length(\"nombre\") BETWEEN 1 AND 100");
        // });
    }
}
