using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_tipo_declaracion")]
[Index("Codigo", Name = "ct_tipo_declaracion_codigo_key", IsUnique = true)]
public partial class CtTipoDeclaracion
{
    [Key]
    [Column("tipo_declaracion_id")]
    public int TipoDeclaracionId { get; set; }

    [Column("codigo")]
    [StringLength(3)]
    public string Codigo { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoDeclaracion")]
    public virtual ICollection<CtDeclaracionesTributaria> CtDeclaracionesTributaria { get; set; } = new List<CtDeclaracionesTributaria>();
}
