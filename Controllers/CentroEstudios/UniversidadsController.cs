using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversidadsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public UniversidadsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Universidads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Universidad>>> GetUniversidad()
        {
            return await _context.Universidad.ToListAsync();
        }

        // GET: api/Universidads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Universidad>> GetUniversidad(int id)
        {
            var universidad = await _context.Universidad.FindAsync(id);

            if (universidad == null)
            {
                return NotFound();
            }

            return universidad;
        }

        // PUT: api/Universidads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUniversidad(int id, Universidad universidad)
        {
            if (id != universidad.Id)
            {
                return BadRequest();
            }

            _context.Entry(universidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UniversidadExists(id))
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

        // POST: api/Universidads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Universidad>> PostUniversidad(Universidad universidad)
        {
            _context.Universidad.Add(universidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUniversidad", new { id = universidad.Id }, universidad);
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUniversidad(int id, [FromBody] Dictionary<string, object> updates)
        {
            var universidad = await _context.Universidad.FindAsync(id);
            if (universidad == null)
            {
                return NotFound();
            }
            foreach (var key in updates.Keys)
            {
                var property = universidad.GetType().GetProperty(key);
                if (property != null && property.CanWrite)
                {
                    var newValue = Convert.ChangeType(updates[key], property.PropertyType);
                    property.SetValue(universidad, newValue);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UniversidadExists(id))
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

        // DELETE: api/Universidads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversidad(int id)
        {
            var universidad = await _context.Universidad.FindAsync(id);
            if (universidad == null)
            {
                return NotFound();
            }

            _context.Universidad.Remove(universidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UniversidadExists(int id)
        {
            return _context.Universidad.Any(e => e.Id == id);
        }
    }
}
