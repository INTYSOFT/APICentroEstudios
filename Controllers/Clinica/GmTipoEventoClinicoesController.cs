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
    public class GmTipoEventoClinicoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmTipoEventoClinicoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmTipoEventoClinicoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmTipoEventoClinico>>> GetGmTipoEventoClinico()
        {
            return await _context.GmTipoEventoClinico.ToListAsync();
        }

        // GET: api/GmTipoEventoClinicoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmTipoEventoClinico>> GetGmTipoEventoClinico(int id)
        {
            var gmTipoEventoClinico = await _context.GmTipoEventoClinico.FindAsync(id);

            if (gmTipoEventoClinico == null)
            {
                return NotFound();
            }

            return gmTipoEventoClinico;
        }

        // PUT: api/GmTipoEventoClinicoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmTipoEventoClinico(int id, GmTipoEventoClinico gmTipoEventoClinico)
        {
            if (id != gmTipoEventoClinico.TipoEventoId)
            {
                return BadRequest();
            }

            _context.Entry(gmTipoEventoClinico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmTipoEventoClinicoExists(id))
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

        // POST: api/GmTipoEventoClinicoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmTipoEventoClinico>> PostGmTipoEventoClinico(GmTipoEventoClinico gmTipoEventoClinico)
        {
            _context.GmTipoEventoClinico.Add(gmTipoEventoClinico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmTipoEventoClinico", new { id = gmTipoEventoClinico.TipoEventoId }, gmTipoEventoClinico);
        }

        // DELETE: api/GmTipoEventoClinicoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmTipoEventoClinico(int id)
        {
            var gmTipoEventoClinico = await _context.GmTipoEventoClinico.FindAsync(id);
            if (gmTipoEventoClinico == null)
            {
                return NotFound();
            }

            _context.GmTipoEventoClinico.Remove(gmTipoEventoClinico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmTipoEventoClinicoExists(int id)
        {
            return _context.GmTipoEventoClinico.Any(e => e.TipoEventoId == id);
        }
    }
}
