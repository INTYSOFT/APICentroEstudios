using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_detalle_lista_ficha_tecnica")]
public partial class LgDetalleListaFichaTecnica
{
    [Key]
    [Column("detalle_lista_ficha_tecnica_id")]
    public int DetalleListaFichaTecnicaId { get; set; }

    [Column("lista_ficha_tecnica_id")]
    public int ListaFichaTecnicaId { get; set; }

    [Column("lista_detalle")]
    [StringLength(50)]
    public string ListaDetalle { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }
}
