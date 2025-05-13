using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;

namespace api_intiSoft.Controllers.Clinica
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmSalasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmSalasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmSalas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmSala>>> GetGmSala()
        {
            return await _context.GmSala.ToListAsync();
        }

        // GET: api/GmSalas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmSala>> GetGmSala(int id)
        {
            var gmSala = await _context.GmSala.FindAsync(id);

            if (gmSala == null)
            {
                return NotFound();
            }

            return gmSala;
        }

        // PUT: api/GmSalas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmSala(int id, GmSala gmSala)
        {
            if (id != gmSala.SalaId)
            {
                return BadRequest();
            }

            _context.Entry(gmSala).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmSalaExists(id))
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

        // POST: api/GmSalas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmSala>> PostGmSala(GmSala gmSala)
        {
            _context.GmSala.Add(gmSala);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmSala", new { id = gmSala.SalaId }, gmSala);
        }

        // DELETE: api/GmSalas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmSala(int id)
        {
            var gmSala = await _context.GmSala.FindAsync(id);
            if (gmSala == null)
            {
                return NotFound();
            }

            _context.GmSala.Remove(gmSala);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmSalaExists(int id)
        {
            return _context.GmSala.Any(e => e.SalaId == id);
        }
    }
}
