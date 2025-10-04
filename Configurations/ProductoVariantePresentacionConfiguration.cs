using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoVariantePresentacionConfiguration : IEntityTypeConfiguration<ProductoVariantePresentacion>
{
    public void Configure(EntityTypeBuilder<ProductoVariantePresentacion> builder)
    {
        // ===== Tabla + check constraints (PostgreSQL) =====
        builder.ToTable("producto_variante_presentacion", "productos", tb =>
        {
            tb.HasCheckConstraint("CK_pvp_precios_pos",
                "(\"precio_compra\" >= 0) AND (\"precio_venta\" >= 0) AND " +
                "(\"costo_flete\"    IS NULL OR \"costo_flete\"    >= 0) AND " +
                "(\"costo_adicional\"IS NULL OR \"costo_adicional\">= 0) AND " +
                "(\"precio_venta_final\" IS NULL OR \"precio_venta_final\" >= 0)");

            tb.HasCheckConstraint("CK_pvp_descuento_pct",
                "\"descuento_porcentaje\" IS NULL OR (\"descuento_porcentaje\" >= 0 AND \"descuento_porcentaje\" <= 100)");

            tb.HasCheckConstraint("CK_pvp_stock_no_neg",
                "(\"stock\" >= 0) AND " +
                "(\"stock_minimo\" IS NULL OR \"stock_minimo\" >= 0) AND " +
                "(\"stock_ideal\"  IS NULL OR \"stock_ideal\"  >= 0) AND " +
                "(\"stock_maximo\" IS NULL OR \"stock_maximo\" >= 0)");
        });

        // ===== Clave =====
        builder.HasKey(x => x.ProductoVariantePresentacionId);

        builder.Property(x => x.ProductoVariantePresentacionId)
               .HasColumnName("producto_variante_presentacion_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoId).HasColumnName("producto_id").IsRequired();
        builder.Property(x => x.ProductoVarianteDetalleId).HasColumnName("producto_variante_detalle_id");
        builder.Property(x => x.ProductoPresentacionId).HasColumnName("producto_presentacion_id");

        builder.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CodigoBarra).HasColumnName("codigo_barra").HasMaxLength(50);
        builder.Property(x => x.CodigoQr).HasColumnName("codigo_qr");
        builder.Property(x => x.CodigoInterno).HasColumnName("codigo_interno").HasMaxLength(100);

        builder.Property(x => x.PrecioCompra).HasColumnName("precio_compra").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.PrecioVenta).HasColumnName("precio_venta").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.CostoFlete).HasColumnName("costo_flete").HasPrecision(10, 2);
        builder.Property(x => x.CostoAdicional).HasColumnName("costo_adicional").HasPrecision(10, 2);
        builder.Property(x => x.DescuentoPorcentaje).HasColumnName("descuento_porcentaje").HasPrecision(5, 2);
        builder.Property(x => x.PrecioVentaFinal).HasColumnName("precio_venta_final").HasPrecision(10, 2);

        builder.Property(x => x.Stock).HasColumnName("stock").IsRequired();
        builder.Property(x => x.StockMinimo).HasColumnName("stock_minimo");
        builder.Property(x => x.StockIdeal).HasColumnName("stock_ideal");
        builder.Property(x => x.StockMaximo).HasColumnName("stock_maximo");

        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();

        builder.Property(x => x.FechaRegistro).HasColumnName("fecha_registro").HasColumnType("timestamp without time zone");
        builder.Property(x => x.FechaActualizacion).HasColumnName("fecha_actualizacion").HasColumnType("timestamp without time zone");

        builder.Property(x => x.UsuaraioRegistroId).HasColumnName("usuaraio_registro_id");
        builder.Property(x => x.UsuaraioActualizacionId).HasColumnName("usuaraio_actualizacion_id");

        builder.Property(x => x.Aliass).HasColumnName("aliass").HasMaxLength(50);
        builder.Property(x => x.NombreProductoCorto).HasColumnName("nombre_producto_corto").HasMaxLength(150);
        builder.Property(x => x.NombreProductoCompleto).HasColumnName("nombre_producto_completo").HasMaxLength(300);
        builder.Property(x => x.NombreProductoComplejo).HasColumnName("nombre_producto_complejo").HasMaxLength(500);
        builder.Property(x => x.EtiquetaVenta).HasColumnName("etiqueta_venta").HasMaxLength(200);

        builder.Property(x => x.NombreBusqueda).HasColumnName("nombre_busqueda").HasColumnType("tsvector");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(64);
        builder.Property(x => x.Descripcion).HasColumnName("descripcion");

        // ===== Índices =====
        builder.HasIndex(x => x.Sku)
               .IsUnique()
               .HasDatabaseName("lg_producto_variante_presentacion_sku_key");

        builder.HasIndex(x => x.ProductoId).HasDatabaseName("IX_pvp_producto");
        builder.HasIndex(x => x.ProductoVarianteDetalleId).HasDatabaseName("IX_pvp_variante_detalle");
        builder.HasIndex(x => x.ProductoPresentacionId).HasDatabaseName("IX_pvp_presentacion");
        builder.HasIndex(x => x.Activo).HasDatabaseName("IX_pvp_activo");
        builder.HasIndex(x => x.Stock).HasDatabaseName("IX_pvp_stock");

        // Full-text search (PostgreSQL)
        builder.HasIndex(x => x.NombreBusqueda)
               .HasMethod("gin")
               .HasDatabaseName("IX_pvp_nombre_busqueda_gin");

        // ===== Relaciones =====

        // (N) -> Producto (1) (requerida)
        builder.HasOne(x => x.Producto)
               .WithMany(p => p.ProductoVariantePresentaciones)
               .HasForeignKey(x => x.ProductoId)
               .OnDelete(DeleteBehavior.Cascade);

        // (N) -> ProductoVarianteDetalle (1) (opcional)
        builder.HasOne(x => x.ProductoVarianteDetalle)
               .WithMany() // o .WithMany(d => d.ProductoVariantePresentaciones) si añades la colección inversa
               .HasForeignKey(x => x.ProductoVarianteDetalleId)
               .OnDelete(DeleteBehavior.SetNull);

        // (N) -> ProductoPresentacion (1) (opcional)
        builder.HasOne(x => x.ProductoPresentacion)
               .WithMany() // o .WithMany(p => p.VariantePresentaciones) si añades colección inversa
               .HasForeignKey(x => x.ProductoPresentacionId)
               .OnDelete(DeleteBehavior.SetNull);

        // (1) -> (N) Detalles
        builder.HasMany(x => x.Detalles)
               .WithOne(d => d.ProductoVariantePresentacion)
               .HasForeignKey(d => d.ProductoVariantePresentacionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
