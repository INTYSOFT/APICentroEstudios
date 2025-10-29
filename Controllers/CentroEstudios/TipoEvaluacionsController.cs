using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEvaluacionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public TipoEvaluacionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/TipoEvaluacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEvaluacion>>> GetTipoEvaluacion()
        {
            return await _context.TipoEvaluacion.ToListAsync();
        }

        // GET: api/TipoEvaluacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEvaluacion>> GetTipoEvaluacion(int id)
        {
            var tipoEvaluacion = await _context.TipoEvaluacion.FindAsync(id);

            if (tipoEvaluacion == null)
            {
                return NotFound();
            }

            return tipoEvaluacion;
        }

        // PUT: api/TipoEvaluacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoEvaluacion(int id, TipoEvaluacion tipoEvaluacion)
        {
            if (id != tipoEvaluacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoEvaluacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEvaluacionExists(id))
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

        // POST: api/TipoEvaluacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoEvaluacion>> PostTipoEvaluacion(TipoEvaluacion tipoEvaluacion)
        {
            _context.TipoEvaluacion.Add(tipoEvaluacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoEvaluacion", new { id = tipoEvaluacion.Id }, tipoEvaluacion);
        }

        // DELETE: api/TipoEvaluacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoEvaluacion(int id)
        {
            var tipoEvaluacion = await _context.TipoEvaluacion.FindAsync(id);
            if (tipoEvaluacion == null)
            {
                return NotFound();
            }

            _context.TipoEvaluacion.Remove(tipoEvaluacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoEvaluacionExists(int id)
        {
            return _context.TipoEvaluacion.Any(e => e.Id == id);
        }
    }
}
