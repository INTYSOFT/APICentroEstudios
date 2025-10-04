using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class CategoriaFichaTecnicaConfiguration : IEntityTypeConfiguration<CategoriaFichaTecnica>
{
    public void Configure(EntityTypeBuilder<CategoriaFichaTecnica> builder)
    {
        // ===== Tabla y clave =====FichaTecnicaId1
        builder.ToTable("categoria_ficha_tecnica", "productos");

        builder.HasKey(x => x.CategoriaFichaTecnicaId);

        builder.Property(x => x.CategoriaFichaTecnicaId)
               .HasColumnName("categoria_ficha_tecnica_id");

        // ===== Columnas =====
        builder.Property(x => x.CategoriaId)
               .HasColumnName("categoria_id");

        builder.Property(x => x.FichaTecnicaId)
               .HasColumnName("ficha_tecnica_id");

        builder.Property(x => x.Activo)
               .HasColumnName("activo");

        // ===== Índices recomendados =====
        builder.HasIndex(x => x.CategoriaId)
               .HasDatabaseName("IX_cft_categoria");

        builder.HasIndex(x => x.FichaTecnicaId)
               .HasDatabaseName("IX_cft_ficha_tecnica");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_cft_activo");

        // (Opcional) Garantizar unicidad por par Categoria–FichaTecnica
        // Descomenta si en tu negocio no puede haber más de un registro por par:
         builder.HasIndex(x => new { x.CategoriaId, x.FichaTecnicaId })
                .IsUnique()
                .HasDatabaseName("UX_cft_categoria_ficha");

        // ===== Relaciones =====

        // CFT -> Categoria (N:1, opcional)
        // 'Categorias' ya expone ICollection<CategoriaFichaTecnica> CategoriaFichaTecnicas
        builder.HasOne(x => x.Categoria)
               .WithMany(c => c.CategoriaFichaTecnicas)
               .HasForeignKey(x => x.CategoriaId)
               .OnDelete(DeleteBehavior.SetNull);

        // CFT -> FichaTecnica (N:1, opcional)
        // Si FichaTecnica tiene navegación ICollection<CategoriaFichaTecnica>,
        // reemplaza .WithMany() por .WithMany(f => f.CategoriaFichaTecnicas)
        builder.HasOne(x => x.FichaTecnica)
               .WithMany(ft => ft.CategoriaFichaTecnicas)
               .HasForeignKey(x => x.FichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        // CFT (1) -> CategoriaFichaTecnicaDetalle (N)
        // Borra en cascada los detalles al eliminar la cabecera.
        builder.HasMany(x => x.CategoriaFichaTecnicaDetalles)
               .WithOne(d => d.CategoriaFichaTecnica)
               .HasForeignKey(d => d.CategoriaFichaTecnicaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
