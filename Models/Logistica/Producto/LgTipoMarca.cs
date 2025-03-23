using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_marca")]
//[Index("Nombre", Name = "idx_lg_tipo_marca_nombre")]
//[Index("Nombre", Name = "lg_tipo_marca_nombre_key", IsUnique = true)]
public partial class LgTipoMarca
{
    [Key]
    [Column("tipo_marca_id")]
    public int TipoMarcaId { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
}
