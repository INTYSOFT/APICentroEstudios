using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_registro_auditoria")]
public partial class LgRegistroAuditorium
{
    [Key]
    [Column("auditoria_id")]
    public int AuditoriaId { get; set; }

    [Column("nombre_tabla")]
    [StringLength(255)]
    public string NombreTabla { get; set; } = null!;

    [Column("accion")]
    [StringLength(50)]
    public string Accion { get; set; } = null!;

    [Column("fecha_hora", TypeName = "timestamp without time zone")]
    public DateTime FechaHora { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }
}
