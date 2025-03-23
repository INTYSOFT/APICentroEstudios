using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_percepciones_retenciones")]
public partial class CtPercepcionesRetencione
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("proveedor_id")]
    public int? ProveedorId { get; set; }

    [Column("cliente_id")]
    public int? ClienteId { get; set; }

    [Column("tipo")]
    [StringLength(50)]
    public string Tipo { get; set; } = null!;

    [Column("fecha", TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; }

    [Column("monto")]
    [Precision(15, 2)]
    public decimal Monto { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [Column("usuario_id")]
    public int UsuarioId { get; set; }
}
