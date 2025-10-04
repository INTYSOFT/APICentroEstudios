using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Catálogo ofertado por proveedor a nivel de producto/variante/presentación, con precio, moneda, lead time y MOQ.
/// </summary>
[Table("proveedor_producto", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_producto_prov")]
[Index("TenantId", "ProveedorId", "ProductoVariantePresentacionId", "VigenteDesde", Name = "uq_prov_prod", IsUnique = true)]
public partial class ProveedorProducto
{
    [Key]
    [Column("proveedor_producto_id")]
    public Guid ProveedorProductoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("producto_variante_presentacion_id")]
    public int ProductoVariantePresentacionId { get; set; }

    [Column("precio_unitario")]
    [Precision(18, 6)]
    public decimal? PrecioUnitario { get; set; }

    [Column("lead_time_dias")]
    public short? LeadTimeDias { get; set; }

    [Column("moq")]
    public int? Moq { get; set; }

    [Column("vigente_desde")]
    public DateOnly? VigenteDesde { get; set; }

    [Column("vigente_hasta")]
    public DateOnly? VigenteHasta { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public Guid? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public Guid? UsuaraioActualizacionId { get; set; }

    [ForeignKey("ProveedorId")]
    [InverseProperty("ProveedorProductos")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
