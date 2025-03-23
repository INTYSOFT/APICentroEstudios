using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_historial_estado_guia")]
public partial class LgHistorialEstadoGuium
{
    [Key]
    [Column("historial_id")]
    public int HistorialId { get; set; }

    [Column("guia_id")]
    public int GuiaId { get; set; }

    [Column("estado_id")]
    public int EstadoId { get; set; }

    [Column("fecha_cambio", TypeName = "timestamp without time zone")]
    public DateTime FechaCambio { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [ForeignKey("EstadoId")]
    [InverseProperty("LgHistorialEstadoGuia")]
    public virtual LgEstadoGuium Estado { get; set; } = null!;

    [ForeignKey("GuiaId")]
    [InverseProperty("LgHistorialEstadoGuia")]
    public virtual LgGuiaRemision Guia { get; set; } = null!;
}
