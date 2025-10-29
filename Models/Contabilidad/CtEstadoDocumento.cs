using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_estado_documento")]
[Index("Estado", Name = "ct_estado_documento_estado_key", IsUnique = true)]
public partial class CtEstadoDocumento
{
    [Key]
    [Column("estado_documento_id")]
    public int EstadoDocumentoId { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("EstadoDocumento")]
    public virtual ICollection<CtDocumentosTributario> CtDocumentosTributarios { get; set; } = new List<CtDocumentosTributario>();
}
