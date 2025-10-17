using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("notas", Schema = "academia")]
public partial class Nota
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("alumno")]
    public int Alumno { get; set; }

    [Column("nota")]
    [Precision(5, 2)]
    public decimal Nota { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }
}
