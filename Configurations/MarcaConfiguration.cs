using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class MarcaConfiguration : IEntityTypeConfiguration<Marca>
{
    public void Configure(EntityTypeBuilder<Marca> builder)
    {
        // ===== Tabla =====
        builder.ToTable("marca", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.MarcaId);
        builder.Property(x => x.MarcaId).HasColumnName("marca_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.CodigoMarca)
               .HasColumnName("codigo_marca")
               .HasMaxLength(50);

        builder.Property(x => x.LogoUrl)
               .HasColumnName("logo_url")
               .HasMaxLength(255);

        builder.Property(x => x.SitioWeb)
               .HasColumnName("sitio_web")
               .HasMaxLength(255);

        builder.Property(x => x.NumeroRegistro)
               .HasColumnName("numero_registro")
               .HasMaxLength(50);

        builder.Property(x => x.TipoMarcaId)
               .HasColumnName("tipo_marca_id");

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true);

        // ===== Índices (respetando los que tenías) =====
        builder.HasIndex(x => x.CodigoMarca)
               .HasDatabaseName("idx_lg_marca_codigo");

        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("idx_lg_marca_nombre");

        builder.HasIndex(x => x.CodigoMarca)
               .IsUnique()
               .HasDatabaseName("lg_marca_codigo_marca_key");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_marca_activo");

        //productoId


        // ===== (Opcional) Constraints útiles por proveedor =====
        // SQL Server:
        // builder.ToTable("marca", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_marca_nombre_len", "LEN([nombre]) BETWEEN 1 AND 100");
        //     tb.HasCheckConstraint("CK_marca_codigo_len", "([codigo_marca] IS NULL) OR (LEN([codigo_marca]) BETWEEN 1 AND 50)");
        // });
        //
        // PostgreSQL:
        // builder.ToTable("marca", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_marca_nombre_len", "char_length(\"nombre\") BETWEEN 1 AND 100");
        //     tb.HasCheckConstraint("CK_marca_codigo_len", "(\"codigo_marca\" IS NULL) OR (char_length(\"codigo_marca\") BETWEEN 1 AND 50)");
        // });
    }
}
