using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ConceptoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Conceptoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concepto>>> GetConcepto()
        {
            return await _context.Concepto.ToListAsync();
        }

        // GET: api/Conceptoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concepto>> GetConcepto(int id)
        {
            var concepto = await _context.Concepto.FindAsync(id);

            if (concepto == null)
            {
                return NotFound();
            }

            return concepto;
        }

        // PUT: api/Conceptoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcepto(int id, Concepto concepto)
        {
            if (id != concepto.Id)
            {
                return BadRequest();
            }

            _context.Entry(concepto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(id))
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

        // POST: api/Conceptoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concepto>> PostConcepto(Concepto concepto)
        {
            _context.Concepto.Add(concepto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcepto", new { id = concepto.Id }, concepto);
        }
        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchConcepto(int id, Concepto concepto)
        {
            if (id != concepto.Id)
            {
                return BadRequest();
            }
            var existingConcepto = await _context.Concepto.FindAsync(id);
            if (existingConcepto == null)
            {
                return NotFound();
            }
            // Update only the fields that are not null in the incoming object
            _context.Entry(existingConcepto).CurrentValues.SetValues(concepto);
            foreach (var property in _context.Entry(concepto).Properties)
            {
                if (property.CurrentValue == null)
                {
                    _context.Entry(existingConcepto).Property(property.Metadata.Name).IsModified = false;
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(id))
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


        // DELETE: api/Conceptoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcepto(int id)
        {
            var concepto = await _context.Concepto.FindAsync(id);
            if (concepto == null)
            {
                return NotFound();
            }

            _context.Concepto.Remove(concepto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptoExists(int id)
        {
            return _context.Concepto.Any(e => e.Id == id);
        }
    }
}
