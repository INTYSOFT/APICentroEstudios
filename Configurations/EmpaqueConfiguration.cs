using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class EmpaqueConfiguration : IEntityTypeConfiguration<Empaque>
{
    public void Configure(EntityTypeBuilder<Empaque> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("empaque", "productos");
        builder.HasKey(x => x.EmpaqueId);

        builder.Property(x => x.EmpaqueId)
               .HasColumnName("empaque_id");

        // ===== Columnas =====
        builder.Property(x => x.Codigo)
               .HasColumnName("codigo")
               .HasMaxLength(3)
               .IsRequired();

        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(32)
               .IsRequired();

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true); // respeta null en BD, pero por defecto true al insertar

        // ===== Índices =====
        builder.HasIndex(x => x.Codigo)
               .IsUnique()
               .HasDatabaseName("UX_empaque_codigo");

        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_empaque_nombre");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_empaque_activo");

        // ===== (Opcional) Check constraints según proveedor =====
        // SQL Server (enforce uppercase y longitud fija 3):
        // builder.ToTable("empaque", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_empaque_codigo_len", "LEN([codigo]) = 3");
        //     tb.HasCheckConstraint("CK_empaque_codigo_upper", "[codigo] = UPPER([codigo])");
        // });
        //
        // PostgreSQL equivalente:
        // builder.ToTable("empaque", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_empaque_codigo_len", "char_length(\"codigo\") = 3");
        //     tb.HasCheckConstraint("CK_empaque_codigo_upper", "\"codigo\" = upper(\"codigo\")");
        // });
    }
}
