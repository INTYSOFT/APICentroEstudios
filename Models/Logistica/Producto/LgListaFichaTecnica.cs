using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_lista_ficha_tecnica")]
public partial class LgListaFichaTecnica
{
    [Key]
    [Column("lista_ficha_tecnica_id")]
    public int ListaFichaTecnicaId { get; set; }

    [Column("lista")]
    [StringLength(50)]
    public string Lista { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }
}
