using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionTipoPreguntumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionTipoPreguntumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionTipoPreguntums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionTipoPreguntum>>> GetEvaluacionTipoPreguntum()
        {
            return await _context.EvaluacionTipoPreguntum.ToListAsync();
        }

        // GET: api/EvaluacionTipoPreguntums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionTipoPreguntum>> GetEvaluacionTipoPreguntum(int id)
        {
            var evaluacionTipoPreguntum = await _context.EvaluacionTipoPreguntum.FindAsync(id);

            if (evaluacionTipoPreguntum == null)
            {
                return NotFound();
            }

            return evaluacionTipoPreguntum;
        }

        // PUT: api/EvaluacionTipoPreguntums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionTipoPreguntum(int id, EvaluacionTipoPreguntum evaluacionTipoPreguntum)
        {
            if (id != evaluacionTipoPreguntum.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionTipoPreguntum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionTipoPreguntumExists(id))
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

        // POST: api/EvaluacionTipoPreguntums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionTipoPreguntum>> PostEvaluacionTipoPreguntum(EvaluacionTipoPreguntum evaluacionTipoPreguntum)
        {
            _context.EvaluacionTipoPreguntum.Add(evaluacionTipoPreguntum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionTipoPreguntum", new { id = evaluacionTipoPreguntum.Id }, evaluacionTipoPreguntum);
        }

        // DELETE: api/EvaluacionTipoPreguntums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionTipoPreguntum(int id)
        {
            var evaluacionTipoPreguntum = await _context.EvaluacionTipoPreguntum.FindAsync(id);
            if (evaluacionTipoPreguntum == null)
            {
                return NotFound();
            }

            _context.EvaluacionTipoPreguntum.Remove(evaluacionTipoPreguntum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionTipoPreguntumExists(int id)
        {
            return _context.EvaluacionTipoPreguntum.Any(e => e.Id == id);
        }
    }
}
