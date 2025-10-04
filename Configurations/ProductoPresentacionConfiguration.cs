using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoPresentacionConfiguration : IEntityTypeConfiguration<ProductoPresentacion>
{
    public void Configure(EntityTypeBuilder<ProductoPresentacion> builder)
    {
        // ===== Tabla (con precision y constraints si deseas) =====
        builder.ToTable("producto_presentacion", "productos");

        // ===== Clave =====
        builder.HasKey(x => x.ProductoPresentacionId);
        builder.Property(x => x.ProductoPresentacionId).HasColumnName("producto_presentacion_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoId).HasColumnName("producto_id").IsRequired();

        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255);
        builder.Property(x => x.Descripcion).HasColumnName("descripcion");

        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);

        builder.Property(x => x.EmpaqueId).HasColumnName("empaque_id");

        builder.Property(x => x.Largo).HasColumnName("largo").HasPrecision(10, 2);
        builder.Property(x => x.Ancho).HasColumnName("ancho").HasPrecision(10, 2);
        builder.Property(x => x.Altura).HasColumnName("altura").HasPrecision(10, 2);
        builder.Property(x => x.UnidadMedidaLongitudId).HasColumnName("unidad_medida_longitud_id");

        builder.Property(x => x.Peso).HasColumnName("peso").HasPrecision(10, 2);
        builder.Property(x => x.UnidadMedidaPesoId).HasColumnName("unidad_medida_peso_id");

        builder.Property(x => x.CategoriaFichaTecnicaDetalleId).HasColumnName("categoria_ficha_tecnica_detalle_id");

        builder.Property(x => x.IncluyePresentacion).HasColumnName("incluye_presentacion").HasDefaultValue(false);
        builder.Property(x => x.ProductoPresentacionIdIncluye).HasColumnName("producto_presentacion_id_incluye");

        builder.Property(x => x.UnidadMedidaContenidoId).HasColumnName("unidad_medida_contenido_id");
        builder.Property(x => x.Contenido).HasColumnName("contenido").HasPrecision(10, 2);

        builder.Property(x => x.CantidadIncluyePresentacion).HasColumnName("cantidad_incluye_presentacion").HasPrecision(10, 2);

        builder.Property(x => x.Orden).HasColumnName("orden");

        builder.Property(x => x.NombreCompuestoCompleto).HasColumnName("nombre_compuesto_completo").HasMaxLength(512);
        builder.Property(x => x.NombreCompuestoCorto).HasColumnName("nombre_compuesto_corto").HasMaxLength(128);

        builder.Property(x => x.NumeracionGrupo).HasColumnName("numeracion_grupo");
        builder.Property(x => x.NombreGrupo).HasColumnName("nombre_grupo").HasMaxLength(32);

        builder.Property(x => x.TipoUsoProductoId).HasColumnName("tipo_uso_producto_id");
        builder.Property(x => x.CantUnidad).HasColumnName("cant_unidad").HasPrecision(18, 4);

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ProductoId).HasDatabaseName("IX_pp_producto");
        builder.HasIndex(x => x.EmpaqueId).HasDatabaseName("IX_pp_empaque");
        builder.HasIndex(x => x.Activo).HasDatabaseName("IX_pp_activo");
        builder.HasIndex(x => x.CategoriaFichaTecnicaDetalleId).HasDatabaseName("IX_pp_cft_detalle");
        builder.HasIndex(x => x.UnidadMedidaLongitudId).HasDatabaseName("IX_pp_um_longitud");
        builder.HasIndex(x => x.UnidadMedidaPesoId).HasDatabaseName("IX_pp_um_peso");
        builder.HasIndex(x => x.UnidadMedidaContenidoId).HasDatabaseName("IX_pp_um_contenido");
        builder.HasIndex(x => x.NombreCompuestoCompleto).HasDatabaseName("IX_pp_nombre_compuesto");

        // Evitar duplicados de presentación por producto y nombre (opcional)
        // builder.HasIndex(x => new { x.ProductoId, x.Nombre })
        //        .IsUnique()
        //        .HasDatabaseName("UX_pp_producto_nombre");

        // ===== Relaciones =====

        // (N) ProductoPresentacion -> (1) Producto (requerida)
        builder.HasOne(x => x.Producto)
               .WithMany(p => p.ProductoPresentaciones)
               .HasForeignKey(x => x.ProductoId)
               .OnDelete(DeleteBehavior.Cascade); // al eliminar Producto, se eliminan sus presentaciones

        // (N) ProductoPresentacion -> (1) Empaque (opcional)
        builder.HasOne(x => x.Empaque)
               .WithMany()
               .HasForeignKey(x => x.EmpaqueId)
               .OnDelete(DeleteBehavior.SetNull);

        // (N) -> (1) Unidad de medida longitud (opcional)
        builder.HasOne(x => x.UnidadMedidaLongitud)
               .WithMany()
               .HasForeignKey(x => x.UnidadMedidaLongitudId)
               .OnDelete(DeleteBehavior.SetNull);

        // (N) -> (1) Unidad de medida peso (opcional)
        builder.HasOne(x => x.UnidadMedidaPeso)
               .WithMany()
               .HasForeignKey(x => x.UnidadMedidaPesoId)
               .OnDelete(DeleteBehavior.SetNull);

        // (N) -> (1) Unidad de medida contenido (opcional)
        builder.HasOne(x => x.UnidadMedidaContenido)
               .WithMany()
               .HasForeignKey(x => x.UnidadMedidaContenidoId)
               .OnDelete(DeleteBehavior.SetNull);

        // (N) -> (1) CategoriaFichaTecnicaDetalle (opcional)
        builder.HasOne(x => x.CategoriaFichaTecnicaDetalle)
               .WithMany()
               .HasForeignKey(x => x.CategoriaFichaTecnicaDetalleId)
               .OnDelete(DeleteBehavior.SetNull);

        // Autorrelación: una presentación puede incluir a otra presentación
        builder.HasOne(x => x.ProductoPresentacionIncluye)
               .WithMany(x => x.IncluidasEn)
               .HasForeignKey(x => x.ProductoPresentacionIdIncluye)
               .OnDelete(DeleteBehavior.Restrict); // evita ciclos de cascada

        // ===== (Opcional) Check constraints por proveedor =====
        // SQL Server:
        // builder.ToTable("producto_presentacion", "productos", tb =>
        // {
        //     tb.HasCheckConstraint("CK_pp_dim_pos", "([largo] IS NULL OR [largo] >= 0) AND ([ancho] IS NULL OR [ancho] >= 0) AND ([altura] IS NULL OR [altura] >= 0)");
        //     tb.HasCheckConstraint("CK_pp_peso_pos", "([peso] IS NULL OR [peso] >= 0)");
        //     tb.HasCheckConstraint("CK_pp_cont_pos", "([contenido] IS NULL OR [contenido] >= 0) AND ([cantidad_incluye_presentacion] IS NULL OR [cantidad_incluye_presentacion] >= 0)");
        // });
        //
        // PostgreSQL: cambia [] por "" y usa funciones equivalentes.
    }
}
