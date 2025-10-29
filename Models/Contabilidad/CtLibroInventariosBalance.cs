using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_libro_inventarios_balances")]
public partial class CtLibroInventariosBalance
{
    [Key]
    [Column("inventario_id")]
    public int InventarioId { get; set; }

    [Column("fecha", TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; }

    [Column("plan_contable_id")]
    public int PlanContableId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("monto")]
    [Precision(15, 2)]
    public decimal? Monto { get; set; }

    [Column("tipo")]
    [StringLength(50)]
    public string? Tipo { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }
}
