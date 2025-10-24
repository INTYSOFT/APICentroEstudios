using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class NivelsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public NivelsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Nivels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nivel>>> GetNivel()
        {
            return await _context.Nivel.ToListAsync();
        }

        // GET: api/Nivels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nivel>> GetNivel(int id)
        {
            var nivel = await _context.Nivel.FindAsync(id);

            if (nivel == null)
            {
                return NotFound();
            }

            return nivel;
        }

        // PUT: api/Nivels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNivel(int id, Nivel nivel)
        {
            if (id != nivel.Id)
            {
                return BadRequest();
            }

            _context.Entry(nivel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NivelExists(id))
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

        // POST: api/Nivels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nivel>> PostNivel(Nivel nivel)
        {
            _context.Nivel.Add(nivel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNivel", new { id = nivel.Id }, nivel);
        }

        // DELETE: api/Nivels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNivel(int id)
        {
            var nivel = await _context.Nivel.FindAsync(id);
            if (nivel == null)
            {
                return NotFound();
            }

            _context.Nivel.Remove(nivel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NivelExists(int id)
        {
            return _context.Nivel.Any(e => e.Id == id);
        }
    }
}
