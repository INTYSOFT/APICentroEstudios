using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentescoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ParentescoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Parentescoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parentesco>>> GetParentesco()
        {
            return await _context.Parentesco.ToListAsync();
        }

        // GET: api/Parentescoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parentesco>> GetParentesco(int id)
        {
            var parentesco = await _context.Parentesco.FindAsync(id);

            if (parentesco == null)
            {
                return NotFound();
            }

            return parentesco;
        }

        // PUT: api/Parentescoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParentesco(int id, Parentesco parentesco)
        {
            if (id != parentesco.Id)
            {
                return BadRequest();
            }

            _context.Entry(parentesco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentescoExists(id))
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

        // POST: api/Parentescoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parentesco>> PostParentesco(Parentesco parentesco)
        {
            _context.Parentesco.Add(parentesco);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParentesco", new { id = parentesco.Id }, parentesco);
        }

        // DELETE: api/Parentescoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentesco(int id)
        {
            var parentesco = await _context.Parentesco.FindAsync(id);
            if (parentesco == null)
            {
                return NotFound();
            }

            _context.Parentesco.Remove(parentesco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentescoExists(int id)
        {
            return _context.Parentesco.Any(e => e.Id == id);
        }
    }
}
