using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_medico")]
public partial class GmMedico
{
    [Key]
    [Column("medico_id")]
    public int MedicoId { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("apellido_paterno")]
    [StringLength(100)]
    public string ApellidoPaterno { get; set; } = null!;

    [Column("apellido_materno")]
    [StringLength(100)]
    public string? ApellidoMaterno { get; set; }

    [Column("documento_identidad_id")]
    [StringLength(3)]
    public string DocumentoIdentidadId { get; set; } = null!;


    [Required]
    [Column("numero_documento")]
    [StringLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("sexo")]
    [StringLength(1)]
    public string? Sexo { get; set; }

    [Column("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [Required]
    [Column("especialidad_id")]
    public int EspecialidadId { get; set; }

    [Column("colegiatura_codigo")]
    [StringLength(20)]
    public string? ColegiaturaCodigo { get; set; }

    [Column("colegiatura_entidad")]
    [StringLength(100)]
    public string? ColegiaturaEntidad { get; set; }

    [Column("colegiatura_fecha")]
    public DateTime? ColegiaturaFecha { get; set; }

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("direccion")]
    public string? Direccion { get; set; }

    [Column("usuario_id")]
    public int? UsuarioId { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [ForeignKey("EspecialidadId")]
    [InverseProperty("GmMedicos")]
    public virtual GmEspecialidad? Especialidad { get; set; }

    [InverseProperty("Medico")]
    public virtual ICollection<LgCliente> GmCitaMedicas { get; set; } = new List<LgCliente>();

    [InverseProperty("Medico")]
    public virtual ICollection<GmHistoriaClinicaEvento> GmHistoriaClinicaEventos { get; set; } = new List<GmHistoriaClinicaEvento>();
}
