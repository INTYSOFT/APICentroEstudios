using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class DetalleListaFichaTecnicaConfiguration : IEntityTypeConfiguration<DetalleListaFichaTecnica>
{
    public void Configure(EntityTypeBuilder<DetalleListaFichaTecnica> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("detalle_lista_ficha_tecnica", "productos");
        builder.HasKey(x => x.DetalleListaFichaTecnicaId);

        builder.Property(x => x.DetalleListaFichaTecnicaId)
               .HasColumnName("detalle_lista_ficha_tecnica_id");

        // ===== Columnas =====
        builder.Property(x => x.ListaFichaTecnicaId)
               .HasColumnName("lista_ficha_tecnica_id")
               .IsRequired();

        builder.Property(x => x.ListaDetalle)
               .HasColumnName("lista_detalle")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Estado)
               .HasColumnName("estado")
               .IsRequired();

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ListaFichaTecnicaId)
               .HasDatabaseName("IX_dltf_lista");

        builder.HasIndex(x => x.Estado)
               .HasDatabaseName("IX_dltf_estado");

        // Evitar duplicar un mismo valor dentro de la misma lista
        builder.HasIndex(x => new { x.ListaFichaTecnicaId, x.ListaDetalle })
               .IsUnique()
               .HasDatabaseName("UX_dltf_lista_detalle");

        // ===== Relaciones =====
        // Relación (1) DetalleListaFichaTecnica -> (N) ProductoFichaTecnicaDetalle (FK opcional)
        // Debe coincidir con lo que ya definimos en ProductoFichaTecnicaDetalleConfiguration:
        // .HasOne(x => x.DetalleListaFichaTecnica).WithMany().OnDelete(SetNull)
        builder.HasMany(x => x.LgProductoFichaTecnicaDetalles)
               .WithOne(d => d.DetalleListaFichaTecnica)
               .HasForeignKey(d => d.DetalleListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.ListaFichaTecnica)
               .WithMany(l => l.DetalleListaFichaTecnicas)
               .HasForeignKey(d => d.ListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull); // o Restrict/Cascade según tu negocio


        // Nota: Si tu entidad ProductoVarianteDetalle tiene FK y navegación
        // .DetalleListaFichaTecnica, puedes activar también esta relación:
        
         builder.HasMany(x => x.LgProductoVarianteDetalles)
                .WithOne(v => v.DetalleListaFichaTecnica)
                .HasForeignKey(v => v.DetalleListaFichaTecnicaId)
                .OnDelete(DeleteBehavior.SetNull);
    }
}
