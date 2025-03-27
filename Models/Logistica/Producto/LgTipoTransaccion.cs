using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_transaccion")]
public partial class LgTipoTransaccion
{
    [Key]
    [Column("tipo_transaccion_id")]
    public short TipoTransaccionId { get; set; }

    [Column("codigo")]
    [StringLength(10)]
    public string Codigo { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string Descripcion { get; set; } = null!;

    [Column("sunat_codigo")]
    [StringLength(10)]
    public string? SunatCodigo { get; set; }

    //activo
    [Column("activo")]
    public bool? Activo { get; set; }

    
}
