using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_unidad_medida")]
public partial class LgUnidadMedidum
{
    [Key]
    [Column("unidad_medida_id")]
    public int UnidadMedidaId { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("abreviatura")]
    [StringLength(10)]
    public string Abreviatura { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("tipo_unidad_medida_id")]
    public int? TipoUnidadMedidaId { get; set; }

    [InverseProperty("UnidadDestino")]
    public virtual ICollection<LgConversionUnidad> LgConversionUnidadUnidadDestinos { get; set; } = new List<LgConversionUnidad>();

    [InverseProperty("UnidadOrigen")]
    public virtual ICollection<LgConversionUnidad> LgConversionUnidadUnidadOrigens { get; set; } = new List<LgConversionUnidad>();

    
    
    [ForeignKey("TipoUnidadMedidaId")]
    [InverseProperty("LgUnidadMedida")]
    public virtual LgTipoUnidadMedidum? TipoUnidadMedida { get; set; }
}
