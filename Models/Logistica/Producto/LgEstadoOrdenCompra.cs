using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_estado_orden_compra")]
public partial class LgEstadoOrdenCompra
{
    [Key]
    [Column("estado_orden_compra_id")]
    public int EstadoOrdenCompraId { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(100)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("EstadoOrdenCompra")]
    public virtual ICollection<LgOrdenCompra> LgOrdenCompras { get; set; } = new List<LgOrdenCompra>();
}
