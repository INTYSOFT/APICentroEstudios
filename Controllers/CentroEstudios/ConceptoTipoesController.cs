using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptoTipoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ConceptoTipoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/ConceptoTipoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConceptoTipo>>> GetConceptoTipo()
        {
            return await _context.ConceptoTipo.ToListAsync();
        }

        // GET: api/ConceptoTipoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConceptoTipo>> GetConceptoTipo(int id)
        {
            var conceptoTipo = await _context.ConceptoTipo.FindAsync(id);

            if (conceptoTipo == null)
            {
                return NotFound();
            }

            return conceptoTipo;
        }

        // PUT: api/ConceptoTipoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConceptoTipo(int id, ConceptoTipo conceptoTipo)
        {
            if (id != conceptoTipo.Id)
            {
                return BadRequest();
            }

            _context.Entry(conceptoTipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoTipoExists(id))
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

        // POST: api/ConceptoTipoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConceptoTipo>> PostConceptoTipo(ConceptoTipo conceptoTipo)
        {
            _context.ConceptoTipo.Add(conceptoTipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConceptoTipo", new { id = conceptoTipo.Id }, conceptoTipo);
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchConceptoTipo(int id, ConceptoTipo conceptoTipo)
        {
            if (id != conceptoTipo.Id)
            {
                return BadRequest();
            }
            var existingConceptoTipo = await _context.ConceptoTipo.FindAsync(id);
            if (existingConceptoTipo == null)
            {
                return NotFound();
            }
            // Update only the fields that are not null in the incoming object
            _context.Entry(existingConceptoTipo).CurrentValues.SetValues(conceptoTipo);
            foreach (var property in _context.Entry(conceptoTipo).Properties)
            {
                if (property.CurrentValue == null)
                {
                    _context.Entry(existingConceptoTipo).Property(property.Metadata.Name).IsModified = false;
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoTipoExists(id))
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


        // DELETE: api/ConceptoTipoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConceptoTipo(int id)
        {
            var conceptoTipo = await _context.ConceptoTipo.FindAsync(id);
            if (conceptoTipo == null)
            {
                return NotFound();
            }

            _context.ConceptoTipo.Remove(conceptoTipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptoTipoExists(int id)
        {
            return _context.ConceptoTipo.Any(e => e.Id == id);
        }
    }
}
