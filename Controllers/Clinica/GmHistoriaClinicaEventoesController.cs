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
    public class GmHistoriaClinicaEventoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmHistoriaClinicaEventoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmHistoriaClinicaEventoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmHistoriaClinicaEvento>>> GetGmHistoriaClinicaEvento()
        {
            return await _context.GmHistoriaClinicaEvento.ToListAsync();
        }

        // GET: api/GmHistoriaClinicaEventoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmHistoriaClinicaEvento>> GetGmHistoriaClinicaEvento(int id)
        {
            var gmHistoriaClinicaEvento = await _context.GmHistoriaClinicaEvento.FindAsync(id);

            if (gmHistoriaClinicaEvento == null)
            {
                return NotFound();
            }

            return gmHistoriaClinicaEvento;
        }

        // PUT: api/GmHistoriaClinicaEventoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmHistoriaClinicaEvento(int id, GmHistoriaClinicaEvento gmHistoriaClinicaEvento)
        {
            if (id != gmHistoriaClinicaEvento.EventoId)
            {
                return BadRequest();
            }

            _context.Entry(gmHistoriaClinicaEvento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmHistoriaClinicaEventoExists(id))
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

        // POST: api/GmHistoriaClinicaEventoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmHistoriaClinicaEvento>> PostGmHistoriaClinicaEvento(GmHistoriaClinicaEvento gmHistoriaClinicaEvento)
        {
            _context.GmHistoriaClinicaEvento.Add(gmHistoriaClinicaEvento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmHistoriaClinicaEvento", new { id = gmHistoriaClinicaEvento.EventoId }, gmHistoriaClinicaEvento);
        }

        // DELETE: api/GmHistoriaClinicaEventoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmHistoriaClinicaEvento(int id)
        {
            var gmHistoriaClinicaEvento = await _context.GmHistoriaClinicaEvento.FindAsync(id);
            if (gmHistoriaClinicaEvento == null)
            {
                return NotFound();
            }

            _context.GmHistoriaClinicaEvento.Remove(gmHistoriaClinicaEvento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmHistoriaClinicaEventoExists(int id)
        {
            return _context.GmHistoriaClinicaEvento.Any(e => e.EventoId == id);
        }
    }
}
