using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class CategoriaFichaTecnicaDetalleConfiguration : IEntityTypeConfiguration<CategoriaFichaTecnicaDetalle>
{
    public void Configure(EntityTypeBuilder<CategoriaFichaTecnicaDetalle> builder)
    {
        // ===== Tabla y clave =====
        builder.ToTable("categoria_ficha_tecnica_detalle", "productos");

        builder.HasKey(x => x.CategoriaFichaTecnicaDetalleId);

        builder.Property(x => x.CategoriaFichaTecnicaDetalleId)
               .HasColumnName("categoria_ficha_tecnica_detalle_id");

        // ===== Columnas =====
        builder.Property(x => x.ListaFichaTecnicaId)
               .HasColumnName("lista_ficha_tecnica_id");

        builder.Property(x => x.CategoriaFichaTecnicaId)
               .HasColumnName("categoria_ficha_tecnica_id")
               .IsRequired();

        builder.Property(x => x.IsRequerido)
               .HasColumnName("is_requerido")
               .HasDefaultValue(false)
               .IsRequired();

        builder.Property(x => x.WithLista)
               .HasColumnName("with_lista")
               .HasDefaultValue(false)
               .IsRequired();

        builder.Property(x => x.Version)
               .HasColumnName("version")
               .HasDefaultValue(1)
               .IsRequired();

        builder.Property(x => x.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)
               .IsRequired();

        builder.Property(x => x.PermitirIngreso)
               .HasColumnName("permitir_ingreso")
               .HasDefaultValue(true)
               .IsRequired();

        builder.Property(x => x.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(64)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(255);

        builder.Property(x => x.TipoDatoId)
               .HasColumnName("tipo_dato_id");

        // ===== Índices recomendados =====
        builder.HasIndex(x => x.CategoriaFichaTecnicaId)
               .HasDatabaseName("IX_cftd_cabecera");

        builder.HasIndex(x => x.ListaFichaTecnicaId)
               .HasDatabaseName("IX_cftd_lista");

        builder.HasIndex(x => x.TipoDatoId)
               .HasDatabaseName("IX_cftd_tipo_dato");

        builder.HasIndex(x => x.Activo)
               .HasDatabaseName("IX_cftd_activo");

        // (Opcional) Evitar duplicados por cabecera: Nombre + Versión
        // builder.HasIndex(x => new { x.CategoriaFichaTecnicaId, x.Nombre, x.Version })
        //        .IsUnique()
        //        .HasDatabaseName("UX_cftd_cabecera_nombre_version");

        // ===== Relaciones =====

        // Detalle (N) -> Cabecera CategoriaFichaTecnica (1) (requerida)
        // Borrado en cascada: al eliminar cabecera se eliminan sus detalles.
        builder.HasOne(x => x.CategoriaFichaTecnica)
               .WithMany(c => c.CategoriaFichaTecnicaDetalles)
               .HasForeignKey(x => x.CategoriaFichaTecnicaId)
               .OnDelete(DeleteBehavior.Cascade);

        // Detalle (N) -> ListaFichaTecnica (1) (opcional)
        // Si existe la navegación inversa, reemplaza .WithMany() por .WithMany(l => l.CategoriaFichaTecnicaDetalles)
        builder.HasOne(x => x.ListaFichaTecnica)
               .WithMany()
               .HasForeignKey(x => x.ListaFichaTecnicaId)
               .OnDelete(DeleteBehavior.SetNull);

        // Detalle (N) -> UnTipoDato (1) (opcional)
        // Si existe la navegación inversa, reemplaza .WithMany() por .WithMany(t => t.CategoriaFichaTecnicaDetalles)
        builder.HasOne(x => x.TipoDato)
               .WithMany()
               .HasForeignKey(x => x.TipoDatoId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
