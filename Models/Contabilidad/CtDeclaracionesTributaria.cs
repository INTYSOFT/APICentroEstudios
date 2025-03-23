using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_declaraciones_tributarias")]
public partial class CtDeclaracionesTributaria
{
    [Key]
    [Column("declaracion_id")]
    public int DeclaracionId { get; set; }

    [Column("tipo_declaracion_id")]
    public int TipoDeclaracionId { get; set; }

    [Column("periodo")]
    public DateOnly Periodo { get; set; }

    [Column("monto_pagado")]
    [Precision(15, 2)]
    public decimal MontoPagado { get; set; }

    [Column("fecha_presentacion", TypeName = "timestamp without time zone")]
    public DateTime FechaPresentacion { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [ForeignKey("TipoDeclaracionId")]
    [InverseProperty("CtDeclaracionesTributaria")]
    public virtual CtTipoDeclaracion TipoDeclaracion { get; set; } = null!;
}
