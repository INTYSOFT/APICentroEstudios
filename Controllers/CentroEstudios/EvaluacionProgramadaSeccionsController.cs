using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionProgramadaSeccionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionProgramadaSeccionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionProgramadaSeccions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadaSeccion>>> GetEvaluacionProgramadaSeccion()
        {
            return await _context.EvaluacionProgramadaSeccion.ToListAsync();
        }

        // GET: api/EvaluacionProgramadaSeccions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionProgramadaSeccion>> GetEvaluacionProgramadaSeccion(int id)
        {
            var evaluacionProgramadaSeccion = await _context.EvaluacionProgramadaSeccion.FindAsync(id);

            if (evaluacionProgramadaSeccion == null)
            {
                return NotFound();
            }

            return evaluacionProgramadaSeccion;
        }

        //GET por evaluacion_programada_id
        [HttpGet("evaluacion_programada/{evaluacion_programada_id}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadaSeccion>>> GetEvaluacionProgramadaSeccionByEvaluacionProgramadaId(int evaluacion_programada_id)
        {
            var evaluacionProgramadaSeccions = await _context.EvaluacionProgramadaSeccion
                .Where(eps => eps.EvaluacionProgramadaId == evaluacion_programada_id)
                .ToListAsync();
            if (evaluacionProgramadaSeccions == null || evaluacionProgramadaSeccions.Count == 0)
            {
                return NotFound();
            }
            return evaluacionProgramadaSeccions;
        }


        //GET por seccion_ciclo_id
        [HttpGet("seccion_ciclo/{seccion_ciclo_id}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadaSeccion>>> GetEvaluacionProgramadaSeccionBySeccionCicloId(int seccion_ciclo_id)
        {
            var evaluacionProgramadaSeccions = await _context.EvaluacionProgramadaSeccion
                .Where(eps => eps.SeccionCicloId == seccion_ciclo_id)
                .ToListAsync();
            if (evaluacionProgramadaSeccions == null || evaluacionProgramadaSeccions.Count == 0)
            {
                return NotFound();
            }
            return evaluacionProgramadaSeccions;
        }

        //GET por seccion_id
        [HttpGet("seccion/{seccion_id}")]
        public async Task<ActionResult<IEnumerable<EvaluacionProgramadaSeccion>>> GetEvaluacionProgramadaSeccionBySeccionId(int seccion_id)
        {
            var evaluacionProgramadaSeccions = await _context.EvaluacionProgramadaSeccion
                .Where(eps => eps.SeccionId == seccion_id)
                .ToListAsync();
            if (evaluacionProgramadaSeccions == null || evaluacionProgramadaSeccions.Count == 0)
            {
                return NotFound();
            }
            return evaluacionProgramadaSeccions;
        }

        // PUT: api/EvaluacionProgramadaSeccions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionProgramadaSeccion(int id, EvaluacionProgramadaSeccion evaluacionProgramadaSeccion)
        {
            if (id != evaluacionProgramadaSeccion.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionProgramadaSeccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionProgramadaSeccionExists(id))
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

        // POST: api/EvaluacionProgramadaSeccions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionProgramadaSeccion>> PostEvaluacionProgramadaSeccion(EvaluacionProgramadaSeccion evaluacionProgramadaSeccion)
        {
            _context.EvaluacionProgramadaSeccion.Add(evaluacionProgramadaSeccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionProgramadaSeccion", new { id = evaluacionProgramadaSeccion.Id }, evaluacionProgramadaSeccion);
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEvaluacionProgramadaSeccion(int id, EvaluacionProgramadaSeccion evaluacionProgramadaSeccion)
        {
            if (id != evaluacionProgramadaSeccion.Id)
            {
                return BadRequest();
            }
            _context.Entry(evaluacionProgramadaSeccion).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionProgramadaSeccionExists(id))
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


        // DELETE: api/EvaluacionProgramadaSeccions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionProgramadaSeccion(int id)
        {
            var evaluacionProgramadaSeccion = await _context.EvaluacionProgramadaSeccion.FindAsync(id);
            if (evaluacionProgramadaSeccion == null)
            {
                return NotFound();
            }

            _context.EvaluacionProgramadaSeccion.Remove(evaluacionProgramadaSeccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionProgramadaSeccionExists(int id)
        {
            return _context.EvaluacionProgramadaSeccion.Any(e => e.Id == id);
        }
    }
}
