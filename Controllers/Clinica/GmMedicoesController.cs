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
    public class GmMedicoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmMedicoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmMedicoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmMedico>>> GetGmMedico()
        {
            return await _context.GmMedico.ToListAsync();
        }

        // GET: api/GmMedicoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmMedico>> GetGmMedico(int id)
        {
            var gmMedico = await _context.GmMedico.FindAsync(id);

            if (gmMedico == null)
            {
                return NotFound();
            }

            return gmMedico;
        }

        //get medicos por especialidad
        [HttpGet("getmedicoses/{especialidadId}")]
        public async Task<ActionResult<IEnumerable<GmMedico>>> GetMedicosPorEspecialidad(int especialidadId)
        {
            var gmMedico = await _context.GmMedico
                .Where(m => m.EspecialidadId == especialidadId)
                .ToListAsync();
            
            return gmMedico;
        }

        // PUT: api/GmMedicoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmMedico(int id, GmMedico gmMedico)
        {
            if (id != gmMedico.MedicoId)
            {
                return BadRequest();
            }
            // 🔁 Asegura que las fechas sean UTC
            gmMedico.FechaNacimiento = ConvertToUtc(gmMedico.FechaNacimiento);
            gmMedico.ColegiaturaFecha = ConvertToUtc(gmMedico.ColegiaturaFecha);
            gmMedico.FechaRegistro = DateTime.UtcNow; // opcionalmente forzado aquí


            _context.Entry(gmMedico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmMedicoExists(id))
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

        // POST: api/GmMedicoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmMedico>> PostGmMedico(GmMedico gmMedico)
        {
            // 🔁 Asegura que las fechas sean UTC
            gmMedico.FechaNacimiento = ConvertToUtc(gmMedico.FechaNacimiento);
            gmMedico.ColegiaturaFecha = ConvertToUtc(gmMedico.ColegiaturaFecha);
            gmMedico.FechaRegistro = DateTime.UtcNow; // opcionalmente forzado aquí

            _context.GmMedico.Add(gmMedico);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmMedico", new { id = gmMedico.MedicoId }, gmMedico);
        }

        // DELETE: api/GmMedicoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmMedico(int id)
        {
            var gmMedico = await _context.GmMedico.FindAsync(id);
            if (gmMedico == null)
            {
                return NotFound();
            }

            _context.GmMedico.Remove(gmMedico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmMedicoExists(int id)
        {
            return _context.GmMedico.Any(e => e.MedicoId == id);
        }
      

        private static DateTime? ConvertToUtc(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToUniversalTime() : null;
        }
    }
}
