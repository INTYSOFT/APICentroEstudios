using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_libro_diario")]
public partial class CtLibroDiario
{
    [Key]
    [Column("asiento_id")]
    public int AsientoId { get; set; }

    [Column("fecha", TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; }

    [Column("plan_contable_id")]
    public int PlanContableId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("debe")]
    [Precision(15, 2)]
    public decimal? Debe { get; set; }

    [Column("haber")]
    [Precision(15, 2)]
    public decimal? Haber { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }
}
