using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;

namespace api_intiSoft.Controllers.Citas
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmEstadoCitumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmEstadoCitumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmEstadoCitums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmEstadoCitum>>> GetGmEstadoCitum()
        {
            return await _context.GmEstadoCitum.ToListAsync();
        }

        // GET: api/GmEstadoCitums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmEstadoCitum>> GetGmEstadoCitum(int id)
        {
            var gmEstadoCitum = await _context.GmEstadoCitum.FindAsync(id);

            if (gmEstadoCitum == null)
            {
                return NotFound();
            }

            return gmEstadoCitum;
        }

        // PUT: api/GmEstadoCitums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmEstadoCitum(int id, GmEstadoCitum gmEstadoCitum)
        {
            if (id != gmEstadoCitum.EstadoCitaId)
            {
                return BadRequest();
            }

            _context.Entry(gmEstadoCitum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmEstadoCitumExists(id))
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

        // POST: api/GmEstadoCitums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmEstadoCitum>> PostGmEstadoCitum(GmEstadoCitum gmEstadoCitum)
        {
            _context.GmEstadoCitum.Add(gmEstadoCitum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmEstadoCitum", new { id = gmEstadoCitum.EstadoCitaId }, gmEstadoCitum);
        }

        // DELETE: api/GmEstadoCitums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmEstadoCitum(int id)
        {
            var gmEstadoCitum = await _context.GmEstadoCitum.FindAsync(id);
            if (gmEstadoCitum == null)
            {
                return NotFound();
            }

            _context.GmEstadoCitum.Remove(gmEstadoCitum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmEstadoCitumExists(int id)
        {
            return _context.GmEstadoCitum.Any(e => e.EstadoCitaId == id);
        }
    }
}
