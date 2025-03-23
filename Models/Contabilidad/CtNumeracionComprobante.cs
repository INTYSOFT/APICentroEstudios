using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_numeracion_comprobante")]
public partial class CtNumeracionComprobante
{
    [Key]
    [Column("numeracion_comprobante_id")]
    public int NumeracionComprobanteId { get; set; }

    [Column("comprobante_id")]
    [StringLength(2)]
    public string ComprobanteId { get; set; } = null!;

    [Column("serie")]
    [StringLength(4)]
    public string Serie { get; set; } = null!;

    [Column("numero_inicial")]
    public int NumeroInicial { get; set; }

    [Column("numero_final")]
    public int NumeroFinal { get; set; }

    [Column("numero_actual")]
    public int NumeroActual { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("ComprobanteId")]
    [InverseProperty("CtNumeracionComprobantes")]
    public virtual CtComprobante Comprobante { get; set; } = null!;
}
