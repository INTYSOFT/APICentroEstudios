using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Controllers.Clinica
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmHorarioMedicoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmHorarioMedicoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmHorarioMedicoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmHorarioMedico>>> GetGmHorarioMedico()
        {
            return await _context.GmHorarioMedico.ToListAsync();
        }

        // GET: api/GmHorarioMedicoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmHorarioMedico>> GetGmHorarioMedico(int id)
        {
            var gmHorarioMedico = await _context.GmHorarioMedico.FindAsync(id);

            if (gmHorarioMedico == null)
            {
                return NotFound();
            }

            return gmHorarioMedico;
        }

        //Get horarios solo por fecha, la fecha se extrae desde el objeto gmHorarioApwrtura
        //[ForeignKey("HorarioAperturaId")]
        //[InverseProperty("GmHorarioMedicos")]
        //public virtual GmHorarioApertura? HorarioApertura { get; set; } = null!;
        [HttpGet("gethorarios/{fecha}")]
        public async Task<ActionResult<IEnumerable<GmHorarioMedico>>> GetHorariosPorFecha(DateOnly fecha)
        {
            var gmHorarioMedico = await _context.GmHorarioMedico
                .Include(h => h.HorarioApertura)
                .Where(h => h.HorarioApertura != null && h.HorarioApertura.Fecha == fecha)
                .ToListAsync();

            return Ok(gmHorarioMedico);
        }





        // PUT: api/GmHorarioMedicoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmHorarioMedico(int id, GmHorarioMedico gmHorarioMedico)
        {
            if (id != gmHorarioMedico.HorarioMedicoId)
            {
                return BadRequest();
            }

            _context.Entry(gmHorarioMedico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmHorarioMedicoExists(id))
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

        // POST: api/GmHorarioMedicoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmHorarioMedico>> PostGmHorarioMedico(GmHorarioMedico gmHorarioMedico)
        {
            // Asegurar que las fechas no incluyan información de zona horaria
            if (gmHorarioMedico.HorarioApertura?.Fecha.HasValue == true)
            {
                gmHorarioMedico.HorarioApertura.Fecha = DateOnly.FromDateTime(gmHorarioMedico.HorarioApertura.Fecha.Value.ToDateTime(TimeOnly.MinValue));
            }
            gmHorarioMedico.FechaRegistro = gmHorarioMedico.FechaRegistro.HasValue
                ? DateTime.SpecifyKind(gmHorarioMedico.FechaRegistro.Value, DateTimeKind.Unspecified)
                : null;

            _context.GmHorarioMedico.Add(gmHorarioMedico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmHorarioMedico", new { id = gmHorarioMedico.HorarioMedicoId }, gmHorarioMedico);
        }

        // DELETE: api/GmHorarioMedicoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmHorarioMedico(int id)
        {
            var gmHorarioMedico = await _context.GmHorarioMedico.FindAsync(id);
            if (gmHorarioMedico == null)
            {
                return NotFound();
            }

            _context.GmHorarioMedico.Remove(gmHorarioMedico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmHorarioMedicoExists(int id)
        {
            return _context.GmHorarioMedico.Any(e => e.HorarioMedicoId == id);
        }
    }
}
