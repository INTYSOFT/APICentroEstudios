using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ListaFichaTecnicaConfiguration : IEntityTypeConfiguration<ListaFichaTecnica>
{
    public void Configure(EntityTypeBuilder<ListaFichaTecnica> builder)
    {
        // ===== Tabla =====
        builder.ToTable("lista_ficha_tecnica", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ListaFichaTecnicaId);
        builder.Property(x => x.ListaFichaTecnicaId)
               .HasColumnName("lista_ficha_tecnica_id");

        // ===== Columnas =====
        builder.Property(x => x.Lista)
               .HasColumnName("lista")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Estado)
               .HasColumnName("estado")
               .IsRequired();

        builder.Property(x => x.TipoListaId)
               .HasColumnName("tipo_lista_id")
               .IsRequired();

        // ===== Índices útiles =====
        builder.HasIndex(x => x.Estado)
               .HasDatabaseName("IX_lft_estado");

        builder.HasIndex(x => x.TipoListaId)
               .HasDatabaseName("IX_lft_tipo");

        builder.HasIndex(x => x.Lista)
               .HasDatabaseName("IX_lft_lista");

        // (Opcional) Evitar duplicados de nombre por tipo de lista:
        // builder.HasIndex(x => new { x.TipoListaId, x.Lista })
        //        .IsUnique()
        //        .HasDatabaseName("UX_lft_tipo_lista");

        // ===== Relaciones =====
        // Lista (1) -> (N) DetalleListaFichaTecnica (FK opcional)
        // Debe coincidir con lo que definimos en DetalleListaFichaTecnicaConfiguration:
        // .HasOne(d => d.ListaFichaTecnica).WithMany().OnDelete(SetNull)
        builder.HasMany(x => x.DetalleListaFichaTecnicas)
               .WithOne(d => d.ListaFichaTecnica)
               .HasForeignKey(d => d.ListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

       

    }
}
