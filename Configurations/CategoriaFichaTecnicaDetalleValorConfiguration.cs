using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class CategoriaFichaTecnicaDetalleValorConfiguration : IEntityTypeConfiguration<CategoriaFichaTecnicaDetalleValor>
{
    public void Configure(EntityTypeBuilder<CategoriaFichaTecnicaDetalleValor> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("categoria_ficha_tecnica_detalle_valor", "productos");

        builder.HasKey(x => x.CategoriaFichaTecnicaDetalleValorId);

        builder.Property(x => x.CategoriaFichaTecnicaDetalleValorId)
               .HasColumnName("categoria_ficha_tecnica_detalle_valor_id");

        // ===== Columnas =====
        builder.Property(x => x.CategoriaFichaTecnicaDetalleId)
               .HasColumnName("categoria_ficha_tecnica_detalle_id")
               .IsRequired();

        builder.Property(x => x.Valor)
               .HasColumnName("valor")
               .HasMaxLength(512)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion");

        builder.Property(x => x.Version)
               .HasColumnName("version")
               .HasDefaultValue(1)
               .IsRequired();

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)
               .IsRequired();

        // ===== Índices recomendados =====
        builder.HasIndex(x => x.CategoriaFichaTecnicaDetalleId)
               .HasDatabaseName("IX_cftdv_detalle");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_cftdv_activo");

        // (Opcional) Evitar duplicados del mismo "Valor" por Detalle + Versión
         builder.HasIndex(x => new { x.CategoriaFichaTecnicaDetalleId, x.Valor, x.Version })
                .IsUnique()
                .HasDatabaseName("UX_cftdv_detalle_valor_version");
               // ===== Relaciones =====

        // Valor (N) -> Detalle (1) (requerida)
        // Borrado en cascada: si se elimina el Detalle, se eliminan sus valores permitidos.
        builder.HasOne(x => x.CategoriaFichaTecnicaDetalle)
               .WithMany() // o .WithMany(d => d.CategoriaFichaTecnicaDetalleValores) si tienes la colección en el modelo Detalle
               .HasForeignKey(x => x.CategoriaFichaTecnicaDetalleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
