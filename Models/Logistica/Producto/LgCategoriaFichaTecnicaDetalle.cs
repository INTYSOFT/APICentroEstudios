using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_intiSoft.Models.Universal;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_categoria_ficha_tecnica_detalle")]
public partial class LgCategoriaFichaTecnicaDetalle
{
    [Key]
    [Column("categoria_ficha_tecnica_detalle_id")]
    public int CategoriaFichaTecnicaDetalleId { get; set; }

    //lista_ficha_tecnica_id
    [ForeignKey("ListaFichaTecnica")]
    [Column("lista_ficha_tecnica_id")]
    public int? ListaFichaTecnicaId { get; set; }

    [ForeignKey("CategoriaFichaTecnica")]
    [Column("categoria_ficha_tecnica_id")]
    public int CategoriaFichaTecnicaId { get; set; }
    public virtual LgCategoriaFichaTecnica? CategoriaFichaTecnica { get; set; }

    [Required]
    [Column("is_requerido")]
    public bool? IsRequerido { get; set; } = false;

    [Required]
    [Column("with_lista")]
    public bool? WithLista { get; set; } = false;

    [Required]
    [Column("version")]
    public int? Version { get; set; } = 1;

    [Required]
    [Column("activo")]
    public bool Activo { get; set; } = true;

    //permitir_ingreso
    [Required]
    [Column("permitir_ingreso")]
    public bool? PermitirIngreso { get; set; } = true; //Si tiene lista si esta activo indica que puede ingresar un valor, de l ocontrario tiene que seleccionarlo de la lista.

    [Column("nombre")]
    [StringLength(64)]
    public string Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    //tipo_dato_id
    [ForeignKey("TipoDato")]
    [Column("tipo_dato_id")]
    public int? TipoDatoId { get; set; }
    public virtual UnTipoDato? TipoDato { get; set; }

}
