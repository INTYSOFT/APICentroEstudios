using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaItemsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public MatriculaItemsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/MatriculaItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaItem>>> GetMatriculaItem()
        {
            return await _context.MatriculaItem.ToListAsync();
        }

        // GET: api/MatriculaItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaItem>> GetMatriculaItem(int id)
        {
            var matriculaItem = await _context.MatriculaItem.FindAsync(id);

            if (matriculaItem == null)
            {
                return NotFound();
            }

            return matriculaItem;
        }

        // PUT: api/MatriculaItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatriculaItem(int id, MatriculaItem matriculaItem)
        {
            if (id != matriculaItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(matriculaItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaItemExists(id))
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

        // POST: api/MatriculaItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MatriculaItem>> PostMatriculaItem(MatriculaItem matriculaItem)
        {
            _context.MatriculaItem.Add(matriculaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatriculaItem", new { id = matriculaItem.Id }, matriculaItem);
        }

        // DELETE: api/MatriculaItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatriculaItem(int id)
        {
            var matriculaItem = await _context.MatriculaItem.FindAsync(id);
            if (matriculaItem == null)
            {
                return NotFound();
            }

            _context.MatriculaItem.Remove(matriculaItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatriculaItemExists(int id)
        {
            return _context.MatriculaItem.Any(e => e.Id == id);
        }
    }
}
