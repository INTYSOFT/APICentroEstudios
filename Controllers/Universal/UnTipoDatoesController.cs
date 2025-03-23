using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Universal;
using intiSoft;

namespace api_intiSoft.Controllers.Universal
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnTipoDatoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public UnTipoDatoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/UnTipoDatoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnTipoDato>>> GetUnTipoDato()
        {
            return await _context.UnTipoDato.ToListAsync();
        }

        // GET: api/UnTipoDatoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnTipoDato>> GetUnTipoDato(int id)
        {
            var unTipoDato = await _context.UnTipoDato.FindAsync(id);

            if (unTipoDato == null)
            {
                return NotFound();
            }

            return unTipoDato;
        }

        // PUT: api/UnTipoDatoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnTipoDato(int id, UnTipoDato unTipoDato)
        {
            if (id != unTipoDato.TipoDatoId)
            {
                return BadRequest();
            }

            _context.Entry(unTipoDato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnTipoDatoExists(id))
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

        // POST: api/UnTipoDatoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnTipoDato>> PostUnTipoDato(UnTipoDato unTipoDato)
        {
            _context.UnTipoDato.Add(unTipoDato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnTipoDato", new { id = unTipoDato.TipoDatoId }, unTipoDato);
        }

        // DELETE: api/UnTipoDatoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnTipoDato(int id)
        {
            var unTipoDato = await _context.UnTipoDato.FindAsync(id);
            if (unTipoDato == null)
            {
                return NotFound();
            }

            _context.UnTipoDato.Remove(unTipoDato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnTipoDatoExists(int id)
        {
            return _context.UnTipoDato.Any(e => e.TipoDatoId == id);
        }
    }
}
