using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_modelo")]
public partial class LgModelo
{
    [Key]
    [Column("modelo_id")]
    public int ModeloId { get; set; }

    [Column("nombre")]
    [StringLength(256)]
    public string Nombre { get; set; } = null!;

    [Column("codigo_modelo")]
    [StringLength(50)]
    public string? CodigoModelo { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime FechaRegistro { get; set; }

    [Column("fecha_actualizacion", TypeName = "timestamp without time zone")]
    public DateTime FechaActualizacion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }
}
