using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api_intiSoft.DTO.Consulta;
using intiSoft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.Consulta;

[ApiController]
[Route("consulta/evaluaciones-programadas")]
public sealed class EvaluacionesProgramadasController : ControllerBase
{
    private readonly ConecDinamicaContext _context;

    public EvaluacionesProgramadasController(ConecDinamicaContext context)
    {
        _context = context;
    }

    [HttpGet("{evaluacionProgramadaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<EvaluacionProgramadaConsultaDto>>> ObtenerPorEvaluacionProgramadaId(
        int evaluacionProgramadaId,
        CancellationToken cancellationToken)
    {
        var resultados = await _context.Evaluacion
            .AsNoTracking()
            .Where(e => e.EvaluacionProgramadaId == evaluacionProgramadaId)
            .Select(e => new EvaluacionProgramadaConsultaDto
            {
                EstadoId = e.EvaluacionProgramada != null ? e.EvaluacionProgramada.EstadoId : null,
                EvaluacionId = e.Id,
                EvaluacionProgramadaId = e.EvaluacionProgramadaId,
                Sede = e.Sede != null ? e.Sede.Nombre : null,
                Ciclo = e.Ciclo != null ? e.Ciclo.Nombre : null,
                Seccion = e.Seccion != null ? e.Seccion.Nombre : null,
                AlumnoDni = e.Alumno != null ? e.Alumno.Dni : null,
                AlumnoApellidos = e.Alumno != null ? e.Alumno.Apellidos : null,
                AlumnoNombres = e.Alumno != null ? e.Alumno.Nombres : null,
                AlumnoCelular = e.Alumno != null ? e.Alumno.Celular : null
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (resultados.Count == 0)
        {
            //return vacio
            return resultados = new List<EvaluacionProgramadaConsultaDto>();
        }

        return Ok(resultados);
    }
}
