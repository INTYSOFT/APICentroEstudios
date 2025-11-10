using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionProgramadumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionProgramadumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionProgramadums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadum()
        {
            return await _context.EvaluacionProgramadum.ToListAsync();
        }

        // GET: api/EvaluacionProgramadums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionProgramadum>> GetEvaluacionProgramadum(int id)
        {
            var evaluacionProgramadum = await _context.EvaluacionProgramadum.FindAsync(id);

            if (evaluacionProgramadum == null)
            {
                return new ActionResult<EvaluacionProgramadum>(NotFound());
            }

            return evaluacionProgramadum;
        }

        //get: por fechainicio
        [HttpGet("fechaInicio/{fecha_inicio}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByFechaInicio(DateOnly fecha_inicio)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio == fecha_inicio)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                //reteurn evaluacionProgramadums vacio
                return evaluacionProgramadums = new List<EvaluacionProgramadum>();



            }
            return evaluacionProgramadums;
        }

        //get: fechainicio y ciclo_id
        [HttpGet("fechaInicio/{fecha_inicio}/ciclo/{ciclo_id}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByFechaInicioAndCiclo(DateOnly fecha_inicio, int ciclo_id)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio == fecha_inicio && e.CicloId == ciclo_id)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new ActionResult<IEnumerable<EvaluacionProgramadum>>(NotFound());
            }
            return evaluacionProgramadums;
        }

        //get: rango de fehchainicio
        [HttpGet("fechaInicio/rango")]
            public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByRangoFechaInicio(
        [FromQuery] DateOnly fechaInicioDesde,
        [FromQuery] DateOnly fechaInicioHasta)        
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio >= fechaInicioDesde && e.FechaInicio <= fechaInicioHasta)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new ActionResult<IEnumerable<EvaluacionProgramadum>>(NotFound());
            }
            return evaluacionProgramadums;
        }

        //GET: activo/año y mes
        [HttpGet("activo/{anio}/{mes}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumActivoByAnioMes(int anio, int mes)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.Activo == true && e.FechaInicio.Year == anio && e.FechaInicio.Month == mes)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new ActionResult<IEnumerable<EvaluacionProgramadum>>(NotFound());
            }
            return evaluacionProgramadums;
        }

        //GEt por estadoId
        [HttpGet("estado/{estadoId}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByEstadoId(int estadoId)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.EstadoId == estadoId)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new ActionResult<IEnumerable<EvaluacionProgramadum>>(NotFound());
            }
            return evaluacionProgramadums;
        }


        // PUT: api/EvaluacionProgramadums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionProgramadum(int id, EvaluacionProgramadum evaluacionProgramadum)
        {
            if (id != evaluacionProgramadum.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionProgramadum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionProgramadumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEvaluacionProgramadum(int id, EvaluacionProgramadum evaluacionProgramadum)
        {
            if (id != evaluacionProgramadum.Id)
            {
                return BadRequest();
            }
            _context.Entry(evaluacionProgramadum).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionProgramadumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/EvaluacionProgramadums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionProgramadum>> PostEvaluacionProgramadum(EvaluacionProgramadum evaluacionProgramadum)
        {
            _context.EvaluacionProgramadum.Add(evaluacionProgramadum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionProgramadum", new { id = evaluacionProgramadum.Id }, evaluacionProgramadum);
        }

        // DELETE: api/EvaluacionProgramadums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionProgramadum(int id)
        {
            var evaluacionProgramadum = await _context.EvaluacionProgramadum.FindAsync(id);
            if (evaluacionProgramadum == null)
            {
                return NotFound();
            }

            _context.EvaluacionProgramadum.Remove(evaluacionProgramadum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionProgramadumExists(int id)
        {
            return _context.EvaluacionProgramadum.Any(e => e.Id == id);
        }
    }
}
