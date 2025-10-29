using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class CicloesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public CicloesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Cicloes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciclo>>> GetCiclo()
        {
            return await _context.Ciclo.ToListAsync();
        }

        // GET: api/Cicloes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ciclo>> GetCiclo(int id)
        {
            var ciclo = await _context.Ciclo.FindAsync(id);

            if (ciclo == null)
            {
                return NotFound();
            }

            return ciclo;
        }

        // PUT: api/Cicloes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiclo(int id, Ciclo ciclo)
        {
            if (id != ciclo.Id)
            {
                return BadRequest();
            }

            _context.Entry(ciclo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CicloExists(id))
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

        // POST: api/Cicloes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ciclo>> PostCiclo(Ciclo ciclo)
        {
            _context.Ciclo.Add(ciclo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCiclo", new { id = ciclo.Id }, ciclo);
        }

        //patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCiclo(int id, Ciclo ciclo)
        {
            if (id != ciclo.Id)
            {
                return BadRequest();
            }
            _context.Entry(ciclo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CicloExists(id))
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

        // DELETE: api/Cicloes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiclo(int id)
        {
            var ciclo = await _context.Ciclo.FindAsync(id);
            if (ciclo == null)
            {
                return NotFound();
            }

            _context.Ciclo.Remove(ciclo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CicloExists(int id)
        {
            return _context.Ciclo.Any(e => e.Id == id);
        }
    }
}
