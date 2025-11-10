using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionDetalleDefatultsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionDetalleDefatultsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionDetalleDefatults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionDetalleDefatult>>> GetEvaluacionDetalleDefatult()
        {
            return await _context.EvaluacionDetalleDefatult.ToListAsync();
        }

        // GET: api/EvaluacionDetalleDefatults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionDetalleDefatult>> GetEvaluacionDetalleDefatult(int id)
        {
            var evaluacionDetalleDefatult = await _context.EvaluacionDetalleDefatult.FindAsync(id);

            if (evaluacionDetalleDefatult == null)
            {
                return NotFound();
            }

            return evaluacionDetalleDefatult;
        }

        // PUT: api/EvaluacionDetalleDefatults/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionDetalleDefatult(int id, EvaluacionDetalleDefatult evaluacionDetalleDefatult)
        {
            if (id != evaluacionDetalleDefatult.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionDetalleDefatult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionDetalleDefatultExists(id))
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

        // POST: api/EvaluacionDetalleDefatults
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionDetalleDefatult>> PostEvaluacionDetalleDefatult(EvaluacionDetalleDefatult evaluacionDetalleDefatult)
        {
            _context.EvaluacionDetalleDefatult.Add(evaluacionDetalleDefatult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionDetalleDefatult", new { id = evaluacionDetalleDefatult.Id }, evaluacionDetalleDefatult);
        }

        // DELETE: api/EvaluacionDetalleDefatults/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionDetalleDefatult(int id)
        {
            var evaluacionDetalleDefatult = await _context.EvaluacionDetalleDefatult.FindAsync(id);
            if (evaluacionDetalleDefatult == null)
            {
                return NotFound();
            }

            _context.EvaluacionDetalleDefatult.Remove(evaluacionDetalleDefatult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionDetalleDefatultExists(int id)
        {
            return _context.EvaluacionDetalleDefatult.Any(e => e.Id == id);
        }
    }
}
