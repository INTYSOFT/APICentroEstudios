using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoFichaTecnicaDetalleConfiguration : IEntityTypeConfiguration<ProductoFichaTecnicaDetalle>
{
    public void Configure(EntityTypeBuilder<ProductoFichaTecnicaDetalle> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("producto_ficha_tecnica_detalle", "productos");

        builder.HasKey(x => x.ProductoFichaTecnicaDetalleId);

        builder.Property(x => x.ProductoFichaTecnicaDetalleId)
               .HasColumnName("producto_ficha_tecnica_detalle_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoFichaTecnicaId)
               .HasColumnName("producto_ficha_tecnica_id")
               .IsRequired();

        builder.Property(x => x.DetalleListaFichaTecnicaId)
               .HasColumnName("detalle_lista_ficha_tecnica_id");

        builder.Property(x => x.Dato)
               .HasColumnName("dato")
               .HasMaxLength(512)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .IsRequired();

        // ===== Índices recomendados =====
        builder.Property(x => x.ProductoFichaTecnicaId)
           .HasColumnName("producto_ficha_tecnica_id")
           .IsRequired();


        builder.HasIndex(x => x.DetalleListaFichaTecnicaId)
               .HasDatabaseName("IX_pftd_detalle_lista");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_pftd_activo");

        // (Opcional) Evitar duplicados del mismo "Dato" por cabecera
        // builder.HasIndex(x => new { x.ProductoFichaTecnicaId, x.Dato })
        //        .IsUnique()
        //        .HasDatabaseName("UX_pftd_pft_dato");

        // ===== Relaciones =====

        // Detalle (N) -> Cabecera PFT (1)  (requerida)
        // Enlaza con la colecc. ProductoFichaTecnica.ProductoFichaTecnicaDetalles
        // Borrado en cascada: si eliminas la cabecera, caen sus detalles.
        builder.HasOne(x => x.ProductoFichaTecnica)
               .WithMany(h => h.ProductoFichaTecnicaDetalles)
               .HasForeignKey(x => x.ProductoFichaTecnicaId)
               .OnDelete(DeleteBehavior.Cascade);

        // Detalle (N) -> DetalleListaFichaTecnica (1)  (opcional)
        // Como no conocemos la navegación inversa exacta, usamos WithMany() sin lambda.
        builder.HasOne(x => x.DetalleListaFichaTecnica)
               .WithMany()
               .HasForeignKey(x => x.DetalleListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);


    }
}
