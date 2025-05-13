using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_detalle_venta")]
public partial class LgDetalleVentum
{
    [Key]
    [Column("detalle_venta_id")]
    public int DetalleVentaId { get; set; }

    [Column("venta_id")]
    public int VentaId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unitario")]
    [Precision(10, 2)]
    public decimal PrecioUnitario { get; set; }

    [Column("subtotal")]
    [Precision(10, 2)]
    public decimal? Subtotal { get; set; }

    [Column("descuento")]
    [Precision(10, 2)]
    public decimal? Descuento { get; set; }

    [Column("otros_cargos")]
    [Precision(10, 2)]
    public decimal? OtrosCargos { get; set; }

    [Column("total_igv")]
    [Precision(10, 2)]
    public decimal? TotalIgv { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("producto_variante_id")]
    public int? ProductoVarianteId { get; set; }

    [Column("producto_presentacion_id")]
    public int? ProductoPresentacionId { get; set; }

    // ForeignKey producto_variante
    [ForeignKey("ProductoVarianteId")]
    [InverseProperty("LgDetalleVentas")]
    public virtual LgProductoVariante? ProductoVariante { get; set; }

    [ForeignKey("VentaId")]
    [InverseProperty("LgDetalleVenta")]
    public virtual LgVentum Venta { get; set; } = null!;



}
