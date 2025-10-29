using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_movimiento_stock")]
public partial class LgTipoMovimientoStock
{
    [Key]
    [Column("tipo_movimiento_id")]
    public int TipoMovimientoId { get; set; }

    [Column("tipo_movimiento")]
    [StringLength(50)]
    public string TipoMovimiento { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(100)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoMovimiento")]
    public virtual ICollection<LgMovimientoStock> LgMovimientoStocks { get; set; } = new List<LgMovimientoStock>();
}
