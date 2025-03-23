using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_elemento_cuenta_contable")]
[Index("TipoCuenta", Name = "ct_elemento_cuenta_contable_tipo_cuenta_key", IsUnique = true)]
public partial class CtElementoCuentaContable
{
    [Key]
    [Column("codigo_elemento_contable_id")]
    [StringLength(3)]
    public string CodigoElementoContableId { get; set; } = null!;

    [Column("tipo_cuenta")]
    [StringLength(128)]
    public string TipoCuenta { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("CodigoElementoContable")]
    public virtual ICollection<CtPlanContable> CtPlanContables { get; set; } = new List<CtPlanContable>();
}
