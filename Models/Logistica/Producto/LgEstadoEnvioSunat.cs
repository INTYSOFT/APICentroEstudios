using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_estado_envio_sunat")]
public partial class LgEstadoEnvioSunat
{
    [Key]
    [Column("estado_envio_sunat_id")]
    public short EstadoEnvioSunatId { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [Column("activo")]
    public bool Activo { get; set; }

    [InverseProperty("EstadoEnvioSunat")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}
