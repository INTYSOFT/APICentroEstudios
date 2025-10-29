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
    public class EvaluacionRespuestumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionRespuestumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionRespuestums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionRespuestum>>> GetEvaluacionRespuestum()
        {
            return await _context.EvaluacionRespuestum.ToListAsync();
        }

        // GET: api/EvaluacionRespuestums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionRespuestum>> GetEvaluacionRespuestum(int id)
        {
            var evaluacionRespuestum = await _context.EvaluacionRespuestum.FindAsync(id);

            if (evaluacionRespuestum == null)
            {
                return NotFound();
            }

            return evaluacionRespuestum;
        }

        // PUT: api/EvaluacionRespuestums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionRespuestum(int id, EvaluacionRespuestum evaluacionRespuestum)
        {
            if (id != evaluacionRespuestum.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionRespuestum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionRespuestumExists(id))
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

        // POST: api/EvaluacionRespuestums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionRespuestum>> PostEvaluacionRespuestum(EvaluacionRespuestum evaluacionRespuestum)
        {
            _context.EvaluacionRespuestum.Add(evaluacionRespuestum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionRespuestum", new { id = evaluacionRespuestum.Id }, evaluacionRespuestum);
        }

        // DELETE: api/EvaluacionRespuestums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionRespuestum(int id)
        {
            var evaluacionRespuestum = await _context.EvaluacionRespuestum.FindAsync(id);
            if (evaluacionRespuestum == null)
            {
                return NotFound();
            }

            _context.EvaluacionRespuestum.Remove(evaluacionRespuestum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionRespuestumExists(int id)
        {
            return _context.EvaluacionRespuestum.Any(e => e.Id == id);
        }
    }
}
