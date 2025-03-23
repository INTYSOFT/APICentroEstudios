using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_linea")]
[Index("Codigo", Name = "lg_linea_codigo_key", IsUnique = true)]
public partial class LgLinea
{
    [Key]
    [Column("linea_id")]
    public int LineaId { get; set; }

    [Column("codigo")]
    [StringLength(50)]
    public string Codigo { get; set; } = null!;

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
}
