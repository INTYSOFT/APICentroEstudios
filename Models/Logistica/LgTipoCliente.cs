using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica;

[Table("lg_tipo_cliente")]
[Index("TipoCliente", Name = "lg_tipo_cliente_tipo_cliente_key", IsUnique = true)]
public partial class LgTipoCliente
{
    [Key]
    [Column("tipo_cliente_id")]
    public short TipoClienteId { get; set; }

    [Column("tipo_cliente")]
    [StringLength(20)]
    public string TipoCliente { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
}
