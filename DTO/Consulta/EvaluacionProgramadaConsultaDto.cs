using System;

namespace api_intiSoft.DTO.Consulta;

/// <summary>
/// Representa el resultado resumido de la consulta de evaluaciones programadas.
/// </summary>
public sealed class EvaluacionProgramadaConsultaDto
{
    public int? EstadoId { get; init; }
    public int EvaluacionId { get; init; }
    public int EvaluacionProgramadaId { get; init; }
    public string? Sede { get; init; }
    public string? Ciclo { get; init; }
    public string? Seccion { get; init; }
    public string? AlumnoDni { get; init; }
    public string? AlumnoApellidos { get; init; }
    public string? AlumnoNombres { get; init; }
    public string? AlumnoCelular { get; init; }
}
