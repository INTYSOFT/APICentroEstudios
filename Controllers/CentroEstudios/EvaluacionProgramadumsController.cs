
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
                return NotFound();
            }

            return evaluacionProgramadum;
        }

        //GET: por DateOnly FechaInicio.
        [HttpGet("fechaInicio/{fechaInicio}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByFechaInicio(DateOnly fechaInicio)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio == fechaInicio)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new List<EvaluacionProgramadum>();
            }
            return evaluacionProgramadums;
        }

        //GET: por fechaInicio entre dos fechas, ordenar por fecha de incio descentrendente
        [HttpGet("fechaInicio/rango")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByFechaInicioRango(DateOnly fechaInicioDesde, DateOnly fechaInicioHasta)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio >= fechaInicioDesde && e.FechaInicio <= fechaInicioHasta)
                .OrderByDescending(e => e.FechaInicio)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new List<EvaluacionProgramadum>();
            }
            return evaluacionProgramadums;
        }

        //get : por fecha y cicloId
        [HttpGet("fechaInicio/{fechaInicio}/ciclo/{cicloId}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByFechaYCiclo(DateOnly fechaInicio, int cicloId)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.FechaInicio == fechaInicio && e.CicloId == cicloId)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new List<EvaluacionProgramadum>();
            }
            return evaluacionProgramadums;
        }

        //GET: Obtener todos en activo=true por año y mes
        [HttpGet("activo/{anio}/{mes}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadum>>> GetEvaluacionProgramadumByAnioMesActivo(int anio, int mes)
        {
            var evaluacionProgramadums = await _context.EvaluacionProgramadum
                .Where(e => e.Activo == true && e.FechaInicio.Year == anio && e.FechaInicio.Month == mes)
                .ToListAsync();
            if (evaluacionProgramadums == null || evaluacionProgramadums.Count == 0)
            {
                return new List<EvaluacionProgramadum>();
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

        // POST: api/EvaluacionProgramadums
        [HttpPost]
        public async Task<ActionResult<EvaluacionProgramadum>> PostEvaluacionProgramadum(EvaluacionProgramadum evaluacionProgramadum)
        {
            //establecer estadoId =1
            evaluacionProgramadum.EstadoId = 1;

            _context.EvaluacionProgramadum.Add(evaluacionProgramadum);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEvaluacionProgramadum", new { id = evaluacionProgramadum.Id }, evaluacionProgramadum);
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
