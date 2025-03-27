using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto_servicio")]
public partial class LgProductoServicio
{
    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("dias")]
    public int? Dias { get; set; }

    [Column("horas")]
    public int? Horas { get; set; }

    [Column("minutos")]
    public int? Minutos { get; set; }

    [Column("segundos")]
    public int? Segundos { get; set; }
}
