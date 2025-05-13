using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto_variante")]
[Index("CodigoBarras", Name = "lg_producto_variacion_codigo_barras_key", IsUnique = true)]
[Index("Nombre", Name = "lg_producto_variacion_nombre_key", IsUnique = true)]
[Index("VarianteSku", Name = "lg_producto_variacion_variante_sku_key", IsUnique = true)]
public partial class LgProductoVariante
{
    [Key]
    [Column("producto_variante_id")]
    public int ProductoVarianteId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_sku")]
    [StringLength(64)]
    public string? VarianteSku { get; set; }

    [Column("nombre")]
    [StringLength(64)]
    public string? Nombre { get; set; }

    [Column("descripcon")]
    [StringLength(512)]
    public string? Descripcon { get; set; }

    [Column("codigo_interno")]
    [StringLength(32)]
    public string? CodigoInterno { get; set; }

    [Column("codigo_barras")]
    [StringLength(64)]
    public string? CodigoBarras { get; set; }

    [Column("codigo_qr")]
    public string? CodigoQr { get; set; }

    [Column("gestionar_precio")]
    public bool? GestionarPrecio { get; set; }

    [Column("ficha_tecnica_id")]
    public int? FichaTecnicaId { get; set; }

    [Column("categoria_ficha_tecnica_id")]
    public int? CategoriaFichaTecnicaId { get; set; }

    [Column("categoria_ficha_tecnica_detalle_id")]
    public int? CategoriaFichaTecnicaDetalleId { get; set; }

    [Column("categoria_ficha_tecnica_detalle_valor_id")]
    public int? CategoriaFichaTecnicaDetalleValorId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("producto_presentacion_id")]
    public int? ProductoPresentacionId { get; set; }

    //variante string
    [Column("variante")]
    public string? Variante { get; set; }

    //COLUMN lista_ficha_tecnica_id
    [Column("lista_ficha_tecnica_id")]
    public int? ListaFichaTecnicaId { get; set; }

    //COLUMN detalle_lista_ficha_tecnica_id
    [Column("detalle_lista_ficha_tecnica_id")]
    public int? DetalleListaFichaTecnicaId { get; set; }



    //LgInventario
    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgInventario> LgInventarios { get; set; } = new HashSet<LgInventario>();

    //lg_item_orden_compra
    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgItemOrdenCompra> LgItemOrdenCompras { get; set; } = new HashSet<LgItemOrdenCompra>();
    
    //lg_movimiento_stock
    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgMovimientoStock> LgMovimientoStocks { get; set; } = new HashSet<LgMovimientoStock>();

    //lg_precio
    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgPrecio> LgPrecios { get; set; } = new HashSet<LgPrecio>();
    //lg_promocion_producto
    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgPromocionProducto> LgPromocionProductos { get; set; } = new HashSet<LgPromocionProducto>();

    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgDetalleVentum> LgDetalleVentas { get; set; } = new HashSet<LgDetalleVentum>();



}
