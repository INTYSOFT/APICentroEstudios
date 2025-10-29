using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_orden_compra")]
public partial class LgOrdenCompra
{
    [Key]
    [Column("orden_compra_id")]
    public int OrdenCompraId { get; set; }

    [Column("proveedor_id")]
    public int ProveedorId { get; set; }

    [Column("fecha_orden", TypeName = "timestamp without time zone")]
    public DateTime FechaOrden { get; set; }

    [Column("estado_orden_compra_id")]
    public int EstadoOrdenCompraId { get; set; }

    [Column("monto_total")]
    [Precision(10, 2)]
    public decimal? MontoTotal { get; set; }

    [ForeignKey("EstadoOrdenCompraId")]
    [InverseProperty("LgOrdenCompras")]
    public virtual LgEstadoOrdenCompra EstadoOrdenCompra { get; set; } = null!;

    [InverseProperty("OrdenCompra")]
    public virtual ICollection<LgItemOrdenCompra> LgItemOrdenCompras { get; set; } = new List<LgItemOrdenCompra>();

    [ForeignKey("ProveedorId")]
    [InverseProperty("LgOrdenCompras")]
    public virtual LgProveedor Proveedor { get; set; } = null!;
}
