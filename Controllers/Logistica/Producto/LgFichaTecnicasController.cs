using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgFichaTecnicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgFichaTecnica>>> GetLgFichaTecnica()
        {
            return await _context.LgFichaTecnica.ToListAsync();
        }

        // GET: api/LgFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgFichaTecnica>> GetLgFichaTecnica(int id)
        {
            var lgFichaTecnica = await _context.LgFichaTecnica.FindAsync(id);

            if (lgFichaTecnica == null)
            {
                return NotFound();
            }

            return lgFichaTecnica;
        }

        // PUT: api/LgFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgFichaTecnica(int id, LgFichaTecnica lgFichaTecnica)
        {
            if (id != lgFichaTecnica.FichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgFichaTecnicaExists(id))
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

        // POST: api/LgFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgFichaTecnica>> PostLgFichaTecnica(LgFichaTecnica lgFichaTecnica)
        {
            _context.LgFichaTecnica.Add(lgFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgFichaTecnica", new { id = lgFichaTecnica.FichaTecnicaId }, lgFichaTecnica);
        }

        // DELETE: api/LgFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgFichaTecnica(int id)
        {
            var lgFichaTecnica = await _context.LgFichaTecnica.FindAsync(id);
            if (lgFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgFichaTecnica.Remove(lgFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgFichaTecnicaExists(int id)
        {
            return _context.LgFichaTecnica.Any(e => e.FichaTecnicaId == id);
        }
    }
}
