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
    public class GmHistoriaClinicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmHistoriaClinicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmHistoriaClinicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmHistoriaClinica>>> GetGmHistoriaClinica()
        {
            return await _context.GmHistoriaClinica.ToListAsync();
        }

        // GET: api/GmHistoriaClinicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmHistoriaClinica>> GetGmHistoriaClinica(int id)
        {
            var gmHistoriaClinica = await _context.GmHistoriaClinica.FindAsync(id);

            if (gmHistoriaClinica == null)
            {
                return NotFound();
            }

            return gmHistoriaClinica;
        }

        // PUT: api/GmHistoriaClinicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmHistoriaClinica(int id, GmHistoriaClinica gmHistoriaClinica)
        {
            if (id != gmHistoriaClinica.HistoriaClinicaId)
            {
                return BadRequest();
            }

            _context.Entry(gmHistoriaClinica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmHistoriaClinicaExists(id))
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

        // POST: api/GmHistoriaClinicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmHistoriaClinica>> PostGmHistoriaClinica(GmHistoriaClinica gmHistoriaClinica)
        {
            _context.GmHistoriaClinica.Add(gmHistoriaClinica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmHistoriaClinica", new { id = gmHistoriaClinica.HistoriaClinicaId }, gmHistoriaClinica);
        }

        // DELETE: api/GmHistoriaClinicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmHistoriaClinica(int id)
        {
            var gmHistoriaClinica = await _context.GmHistoriaClinica.FindAsync(id);
            if (gmHistoriaClinica == null)
            {
                return NotFound();
            }

            _context.GmHistoriaClinica.Remove(gmHistoriaClinica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmHistoriaClinicaExists(int id)
        {
            return _context.GmHistoriaClinica.Any(e => e.HistoriaClinicaId == id);
        }
    }
}
