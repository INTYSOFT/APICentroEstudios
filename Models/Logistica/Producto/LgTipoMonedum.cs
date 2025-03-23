using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_moneda")]
public partial class LgTipoMonedum
{
    [Key]
    [Column("tipo_moneda_id")]
    public int TipoMonedaId { get; set; }

    [Column("tipo_moneda")]
    [StringLength(50)]
    public string TipoMoneda { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoMoneda")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}
