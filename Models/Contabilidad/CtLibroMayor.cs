using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_libro_mayor")]
public partial class CtLibroMayor
{
    [Key]
    [Column("mayor_id")]
    public int MayorId { get; set; }

    [Column("plan_contable_id")]
    public int PlanContableId { get; set; }

    [Column("fecha", TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; }

    [Column("saldo_anterior")]
    [Precision(15, 2)]
    public decimal? SaldoAnterior { get; set; }

    [Column("movimientos_periodo", TypeName = "jsonb")]
    public string? MovimientosPeriodo { get; set; }

    [Column("saldo_final")]
    [Precision(15, 2)]
    public decimal? SaldoFinal { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }
}
