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
    public class GmCitaMedicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmCitaMedicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmCitaMedicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCliente>>> GetGmCitaMedica()
        {
            return await _context.GmCitaMedica.ToListAsync();
        }

        // GET: api/GmCitaMedicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCliente>> GetGmCitaMedica(int id)
        {
            var gmCitaMedica = await _context.GmCitaMedica.FindAsync(id);

            if (gmCitaMedica == null)
            {
                return NotFound();
            }

            return gmCitaMedica;
        }

        // PUT: api/GmCitaMedicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmCitaMedica(int id, LgCliente gmCitaMedica)
        {
            if (id != gmCitaMedica.CitaId)
            {
                return BadRequest();
            }

            _context.Entry(gmCitaMedica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmCitaMedicaExists(id))
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

        // POST: api/GmCitaMedicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCliente>> PostGmCitaMedica(LgCliente gmCitaMedica)
        {
            _context.GmCitaMedica.Add(gmCitaMedica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmCitaMedica", new { id = gmCitaMedica.CitaId }, gmCitaMedica);
        }

        // DELETE: api/GmCitaMedicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmCitaMedica(int id)
        {
            var gmCitaMedica = await _context.GmCitaMedica.FindAsync(id);
            if (gmCitaMedica == null)
            {
                return NotFound();
            }

            _context.GmCitaMedica.Remove(gmCitaMedica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmCitaMedicaExists(int id)
        {
            return _context.GmCitaMedica.Any(e => e.CitaId == id);
        }
    }
}
