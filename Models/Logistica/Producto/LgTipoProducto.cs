using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_producto")]
[Index("TipoProducto", Name = "lg_tipo_producto_tipo_producto_key", IsUnique = true)]
public partial class LgTipoProducto
{
    [Key]
    [Column("tipo_producto_id")]
    public short TipoProductoId { get; set; }

    [Column("tipo_producto")]
    [StringLength(32)]
    public string TipoProducto { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(128)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }
}
