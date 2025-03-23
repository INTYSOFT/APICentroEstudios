using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_guia_remision")]
[Index("NumeroGuia", Name = "lg_guia_remision_numero_guia_key", IsUnique = true)]
public partial class LgGuiaRemision
{
    [Key]
    [Column("guia_id")]
    public int GuiaId { get; set; }

    [Column("transportista_id")]
    public int TransportistaId { get; set; }

    [Column("vehiculo_id")]
    public int VehiculoId { get; set; }

    [Column("destinatario_id")]
    public int DestinatarioId { get; set; }

    [Column("numero_guia")]
    [StringLength(20)]
    public string NumeroGuia { get; set; } = null!;

    [Column("fecha_emision", TypeName = "timestamp without time zone")]
    public DateTime FechaEmision { get; set; }

    [Column("fecha_inicio_traslado", TypeName = "timestamp without time zone")]
    public DateTime FechaInicioTraslado { get; set; }

    [Column("fecha_fin_traslado", TypeName = "timestamp without time zone")]
    public DateTime? FechaFinTraslado { get; set; }

    [Column("geo_latitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLatitudOrigen { get; set; }

    [Column("geo_longitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLongitudOrigen { get; set; }

    [Column("punto_partida")]
    [StringLength(255)]
    public string PuntoPartida { get; set; } = null!;

    [Column("geo_latitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLatitudLlegada { get; set; }

    [Column("geo_longitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLongitudLlegada { get; set; }

    [Column("punto_llegada")]
    [StringLength(255)]
    public string PuntoLlegada { get; set; } = null!;

    [Column("motivo_traslado")]
    [StringLength(255)]
    public string MotivoTraslado { get; set; } = null!;

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("DestinatarioId")]
    [InverseProperty("LgGuiaRemisions")]
    public virtual LgDestinatario Destinatario { get; set; } = null!;

    [InverseProperty("Guia")]
    public virtual ICollection<LgDetalleGuiaRemision> LgDetalleGuiaRemisions { get; set; } = new List<LgDetalleGuiaRemision>();

    [InverseProperty("Guia")]
    public virtual ICollection<LgHistorialEstadoGuium> LgHistorialEstadoGuia { get; set; } = new List<LgHistorialEstadoGuium>();

    [ForeignKey("TransportistaId")]
    [InverseProperty("LgGuiaRemisions")]
    public virtual LgTransportistum Transportista { get; set; } = null!;

    [ForeignKey("VehiculoId")]
    [InverseProperty("LgGuiaRemisions")]
    public virtual LgVehiculo Vehiculo { get; set; } = null!;
}
