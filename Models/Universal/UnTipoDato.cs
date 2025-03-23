using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Universal;

[Table("un_tipo_dato")]
public partial class UnTipoDato
{
    [Key]
    [Column("tipo_dato_id")]
    public int TipoDatoId { get; set; }

    [Column("nombre")]
    [StringLength(64)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }
}
