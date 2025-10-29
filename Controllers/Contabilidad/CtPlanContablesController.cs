using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Contabilidad;
using intiSoft;

namespace api_intiSoft.Controllers.Contabilidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtPlanContablesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public CtPlanContablesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/CtPlanContables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CtPlanContable>>> GetCtPlanContable()
        {
            return await _context.CtPlanContable.ToListAsync();
        }

        // GET: api/CtPlanContables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CtPlanContable>> GetCtPlanContable(int id)
        {
            var ctPlanContable = await _context.CtPlanContable.FindAsync(id);

            if (ctPlanContable == null)
            {
                return NotFound();
            }

            return ctPlanContable;
        }

        // PUT: api/CtPlanContables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtPlanContable(int id, CtPlanContable ctPlanContable)
        {
            if (id != ctPlanContable.PlanContableId)
            {
                return BadRequest();
            }

            _context.Entry(ctPlanContable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CtPlanContableExists(id))
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

        // POST: api/CtPlanContables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CtPlanContable>> PostCtPlanContable(CtPlanContable ctPlanContable)
        {
            _context.CtPlanContable.Add(ctPlanContable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCtPlanContable", new { id = ctPlanContable.PlanContableId }, ctPlanContable);
        }

        // DELETE: api/CtPlanContables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCtPlanContable(int id)
        {
            var ctPlanContable = await _context.CtPlanContable.FindAsync(id);
            if (ctPlanContable == null)
            {
                return NotFound();
            }

            _context.CtPlanContable.Remove(ctPlanContable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CtPlanContableExists(int id)
        {
            return _context.CtPlanContable.Any(e => e.PlanContableId == id);
        }
    }
}
