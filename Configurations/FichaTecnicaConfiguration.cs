using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class FichaTecnicaConfiguration : IEntityTypeConfiguration<FichaTecnica>
{
    public void Configure(EntityTypeBuilder<FichaTecnica> builder)
    {
        // ===== Tabla + (opcional) constraints a nivel tabla =====
        builder.ToTable("ficha_tecnica", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.FichaTecnicaId);

        builder.Property(x => x.FichaTecnicaId)
               .HasColumnName("ficha_tecnica_id");

        // ===== Columnas =====
        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(256)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)
               .IsRequired();

        // ===== Índices útiles =====
        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_fichatecnica_activo");

        builder.HasIndex(x => x.Nombre)
               .HasDatabaseName("IX_fichatecnica_nombre");

        // (Opcional) Unicidad del nombre si aplica a tu negocio:
        // builder.HasIndex(x => x.Nombre)
        //        .IsUnique()
        //        .HasDatabaseName("UX_fichatecnica_nombre");

        // ===== Relaciones =====

        // FichaTecnica (1) -> (N) CategoriaFichaTecnica (FK opcional en CFT)
        // Mantiene consistencia con CategoriaFichaTecnicaConfiguration (.OnDelete(SetNull))
        builder.HasMany(ft => ft.CategoriaFichaTecnicas)
               .WithOne(cft => cft.FichaTecnica)
               .HasForeignKey(cft => cft.FichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        // FichaTecnica (1) -> (N) ProductoFichaTecnica (FK opcional en PFT)
        // Mantiene consistencia con ProductoFichaTecnicaConfiguration (.OnDelete(SetNull))
        builder.HasMany(ft => ft.ProductoFichaTecnicas)
               .WithOne(pft => pft.FichaTecnica)
               .HasForeignKey(pft => pft.FichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
