using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_plan_contable")]
[Index("CodigoCuenta", Name = "ct_plan_contable_codigo_cuenta_key", IsUnique = true)]
public partial class CtPlanContable
{
    [Key]
    [Column("plan_contable_id")]
    public int PlanContableId { get; set; }

    [Column("codigo_elemento_contable_id")]
    [StringLength(3)]
    public string CodigoElementoContableId { get; set; } = null!;

    [Column("codigo_cuenta")]
    [StringLength(20)]
    public string CodigoCuenta { get; set; } = null!;

    [Column("nombre_cuenta")]
    [StringLength(255)]
    public string NombreCuenta { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("CodigoElementoContableId")]
    [InverseProperty("CtPlanContables")]
    public virtual CtElementoCuentaContable CodigoElementoContable { get; set; } = null!;
}
