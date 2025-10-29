using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_venta")]
[Index("EstadoEnvioSunatId", Name = "fki_kf_estado_envio_sunat_id_venta")]
public partial class LgVentum
{
    [Key]
    [Column("venta_id")]
    public int VentaId { get; set; }

    [Column("sucursal_id")]
    public int SucursalId { get; set; }

    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("comprobante_id")]
    [StringLength(2)]
    public string ComprobanteId { get; set; } = null!;

    [Column("forma_pago_id")]
    public int FormaPagoId { get; set; }

    [Column("tipo_moneda_id")]
    public int TipoMonedaId { get; set; }

    [Column("nro_serie_comprobante")]
    [StringLength(8)]
    public string? NroSerieComprobante { get; set; }

    [Column("nro_comprobante")]
    [StringLength(16)]
    public string? NroComprobante { get; set; }

    [Column("fecha_orden", TypeName = "timestamp without time zone")]
    public DateTime FechaOrden { get; set; }

    [Column("subtotal")]
    [Precision(10, 2)]
    public decimal? Subtotal { get; set; }

    [Column("total_igv")]
    [Precision(10, 2)]
    public decimal? TotalIgv { get; set; }

    [Column("descuento")]
    [Precision(10, 2)]
    public decimal? Descuento { get; set; }

    [Column("otros_cargos")]
    [Precision(10, 2)]
    public decimal? OtrosCargos { get; set; }

    [Column("total_pagar")]
    [Precision(10, 2)]
    public decimal? TotalPagar { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("estado_envio_sunat_id")]
    public short? EstadoEnvioSunatId { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("LgVenta")]
    public virtual LgCliente Cliente { get; set; } = null!;

    [ForeignKey("EstadoEnvioSunatId")]
    [InverseProperty("LgVenta")]
    public virtual LgEstadoEnvioSunat? EstadoEnvioSunat { get; set; }

    [ForeignKey("FormaPagoId")]
    [InverseProperty("LgVenta")]
    public virtual LgFormaPago FormaPago { get; set; } = null!;

    [InverseProperty("Venta")]
    public virtual ICollection<LgItemVentum> LgItemVenta { get; set; } = new List<LgItemVentum>();

    [ForeignKey("SucursalId")]
    [InverseProperty("LgVenta")]
    public virtual LgSucursal Sucursal { get; set; } = null!;

    [ForeignKey("TipoMonedaId")]
    [InverseProperty("LgVenta")]
    public virtual LgTipoMonedum TipoMoneda { get; set; } = null!;
}
