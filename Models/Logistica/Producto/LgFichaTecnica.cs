using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_ficha_tecnica")]
public partial class LgFichaTecnica
{
    public LgFichaTecnica()
    {
        LgProductoFichaTecnicas = new HashSet<LgProductoFichaTecnica>();
        LgCategoriaFichaTecnicas = new HashSet<LgCategoriaFichaTecnica>();
    }

    [Key]
    [Column("ficha_tecnica_id")]
    public int FichaTecnicaId { get; set; }

    [Column("nombre")]
    [StringLength(256)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    // Relaciones
    public virtual ICollection<LgCategoriaFichaTecnica> LgCategoriaFichaTecnicas { get; set; }
    public virtual ICollection<LgProductoFichaTecnica> LgProductoFichaTecnicas { get; set; }
}








