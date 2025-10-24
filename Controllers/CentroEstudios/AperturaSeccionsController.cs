using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AperturaSeccionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AperturaSeccionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/AperturaSeccions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AperturaSeccion>>> GetAperturaSeccion()
        {
            return await _context.AperturaSeccion.ToListAsync();
        }

        // GET: api/AperturaSeccions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AperturaSeccion>> GetAperturaSeccion(int id)
        {
            var aperturaSeccion = await _context.AperturaSeccion.FindAsync(id);

            if (aperturaSeccion == null)
            {
                return NotFound();
            }

            return aperturaSeccion;
        }

        // PUT: api/AperturaSeccions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAperturaSeccion(int id, AperturaSeccion aperturaSeccion)
        {
            if (id != aperturaSeccion.Id)
            {
                return BadRequest();
            }

            _context.Entry(aperturaSeccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AperturaSeccionExists(id))
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

        // POST: api/AperturaSeccions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AperturaSeccion>> PostAperturaSeccion(AperturaSeccion aperturaSeccion)
        {
            _context.AperturaSeccion.Add(aperturaSeccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAperturaSeccion", new { id = aperturaSeccion.Id }, aperturaSeccion);
        }
        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAperturaSeccion(int id, AperturaSeccion aperturaSeccion)
        {
            if (id != aperturaSeccion.Id)
            {
                return BadRequest();
            }
            var existingAperturaSeccion = await _context.AperturaSeccion.FindAsync(id);
            if (existingAperturaSeccion == null)
            {
                return NotFound();
            }
            // Update only the fields that are not null in the incoming object
            _context.Entry(existingAperturaSeccion).CurrentValues.SetValues(aperturaSeccion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AperturaSeccionExists(id))
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


        // DELETE: api/AperturaSeccions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAperturaSeccion(int id)
        {
            var aperturaSeccion = await _context.AperturaSeccion.FindAsync(id);
            if (aperturaSeccion == null)
            {
                return NotFound();
            }

            _context.AperturaSeccion.Remove(aperturaSeccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AperturaSeccionExists(int id)
        {
            return _context.AperturaSeccion.Any(e => e.Id == id);
        }
    }
}
