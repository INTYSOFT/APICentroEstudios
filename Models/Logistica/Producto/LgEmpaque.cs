using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_empaque")]
public partial class LgEmpaque
{
    [Key]
    [Column("empaque_id")]
    public int EmpaqueId { get; set; }

    [Column("codigo")]
    [StringLength(3)]
    public string Codigo { get; set; } = null!;

    [Column("nombre")]
    [StringLength(32)]
    public string Nombre { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Empaque")]
    public virtual ICollection<LgEmpaqueProducto> LgEmpaqueProductos { get; set; } = new List<LgEmpaqueProducto>();
}
