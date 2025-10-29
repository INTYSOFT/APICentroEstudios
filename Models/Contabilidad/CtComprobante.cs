using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_comprobante")]
public partial class CtComprobante
{
    [Key]
    [Column("comprobante_id")]
    [StringLength(2)]
    public string ComprobanteId { get; set; } = null!;

    [Column("comprobante")]
    [StringLength(255)]
    public string Comprobante { get; set; } = null!;

    [Column("aliass")]
    [StringLength(50)]
    public string? Aliass { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [InverseProperty("Comprobante")]
    public virtual ICollection<CtDocumentosTributario> CtDocumentosTributarios { get; set; } = new List<CtDocumentosTributario>();

    [InverseProperty("Comprobante")]
    public virtual ICollection<CtNumeracionComprobante> CtNumeracionComprobantes { get; set; } = new List<CtNumeracionComprobante>();
}
