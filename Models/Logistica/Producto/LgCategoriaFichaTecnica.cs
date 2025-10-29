using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_categoria_ficha_tecnica")]
public partial class LgCategoriaFichaTecnica
{
    public LgCategoriaFichaTecnica()
    {
        CategoriaFichaTecnicaDetalles = new HashSet<LgCategoriaFichaTecnicaDetalle>();
        //FichaTecnica = new LgFichaTecnica();
        //Categoria = new LgCategoria();

    }

    [Key]
    [Column("categoria_ficha_tecnica_id")]
    public int CategoriaFichaTecnicaId { get; set; }

    [ForeignKey("Categoria")]
    [Column("categoria_id")]
    public int? CategoriaId { get; set; }
    public virtual LgCategoria? Categoria { get; set; }

    [ForeignKey("FichaTecnica")]
    [Column("ficha_tecnica_id")]
    public int? FichaTecnicaId { get; set; }
    public virtual LgFichaTecnica? FichaTecnica { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
    public virtual ICollection<LgCategoriaFichaTecnicaDetalle?> CategoriaFichaTecnicaDetalles { get; set; }
    

}

