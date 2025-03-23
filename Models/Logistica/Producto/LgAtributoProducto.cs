using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_atributo_producto")]
public partial class LgAtributoProducto
{
    [Key]
    [Column("atributo_producto_id")]
    public int AtributoProductoId { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("tipo_dato")]
    [StringLength(50)]
    public string? TipoDato { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    /// <summary>
    /// Si es verdadero, Los detalle se mostrarán en una sola línea.
    /// </summary>
    [Column("is_grupo")]
    public bool? IsGrupo { get; set; }

    /// <summary>
    /// Si es true, indica que tendrá una lista donde se seleccionará un valor, de lo contrario se ingresará un valor.
    /// </summary>
    [Column("with_lista")]
    public bool? WithLista { get; set; }

    /// <summary>
    /// Indica si el atributo es requerido
    /// </summary>
    [Column("is_requerido")]
    public bool IsRequerido { get; set; }

    /// <summary>
    /// Indica si el atributo forma  parte del nombre del producto
    /// </summary>
    [Column("is_parte_nombre")]
    public bool IsParteNombre { get; set; }

    /// <summary>
    /// Si es verdadero, se permitirá seleccionar varios valores.
    /// </summary>
    [Column("is_multiple")]
    public bool IsMultiple { get; set; }

    /// <summary>
    /// Si es verdadero se mostrara el atributo para la empresa, esto será gestionado por INTISoft, para cada cliente.
    /// </summary>
    [Column("is_visible")]
    public bool? IsVisible { get; set; }

    /// <summary>
    /// El valor cuando no se encuentre una lista.
    /// </summary>
    [Column("valor")]
    [StringLength(32)]
    public string? Valor { get; set; }

    /// <summary>
    /// El valor solo cuando sea de tipo de dato sea = check, true o false
    /// </summary>
    [Column("valor_check")]
    public bool? ValorCheck { get; set; }

    [InverseProperty("AtributoProducto")]
    public virtual ICollection<LgAtributoProductoDetalle> LgAtributoProductoDetalles { get; set; } = new List<LgAtributoProductoDetalle>();
}
