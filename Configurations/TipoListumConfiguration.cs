using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class TipoListumConfiguration : IEntityTypeConfiguration<TipoListum>
{
    public void Configure(EntityTypeBuilder<TipoListum> builder)
    {
        // ===== Tabla =====
        builder.ToTable("tipo_lista", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.TipoListaId);

        builder.Property(x => x.TipoListaId)
               .HasColumnName("tipo_lista_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true);

        // ===== Índices útiles =====
        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_tipolista_nombre");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_tipolista_activo");

        // (Opcional) Unicidad por nombre si aplica a tu negocio:
        // builder.HasIndex(x => x.Nombre)
        //        .IsUnique()
        //        .HasDatabaseName("UX_tipolista_nombre");

        // ===== (Opcional) Check constraints (elige sintaxis por proveedor)
        // SQL Server:
        // builder.ToTable("tipo_lista", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_tipolista_nombre_len", "LEN([nombre]) BETWEEN 1 AND 100");
        // });
        //
        // PostgreSQL:
        // builder.ToTable("tipo_lista", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_tipolista_nombre_len", "char_length(\"nombre\") BETWEEN 1 AND 100");
        // });
    }
}
