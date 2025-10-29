using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionDetallesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionDetallesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionDetalle>>> GetEvaluacionDetalle()
        {
            return await _context.EvaluacionDetalle.ToListAsync();
        }

        // GET: api/EvaluacionDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionDetalle>> GetEvaluacionDetalle(int id)
        {
            var evaluacionDetalle = await _context.EvaluacionDetalle.FindAsync(id);

            if (evaluacionDetalle == null)
            {
                return NotFound();
            }

            return evaluacionDetalle;
        }

        // PUT: api/EvaluacionDetalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionDetalle(int id, EvaluacionDetalle evaluacionDetalle)
        {
            if (id != evaluacionDetalle.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionDetalleExists(id))
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

        // POST: api/EvaluacionDetalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionDetalle>> PostEvaluacionDetalle(EvaluacionDetalle evaluacionDetalle)
        {
            _context.EvaluacionDetalle.Add(evaluacionDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionDetalle", new { id = evaluacionDetalle.Id }, evaluacionDetalle);
        }

        // DELETE: api/EvaluacionDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionDetalle(int id)
        {
            var evaluacionDetalle = await _context.EvaluacionDetalle.FindAsync(id);
            if (evaluacionDetalle == null)
            {
                return NotFound();
            }

            _context.EvaluacionDetalle.Remove(evaluacionDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionDetalleExists(int id)
        {
            return _context.EvaluacionDetalle.Any(e => e.Id == id);
        }
    }
}
