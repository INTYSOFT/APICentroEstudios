using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgConversionUnidadsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgConversionUnidadsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgConversionUnidads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgConversionUnidad>>> GetLgConversionUnidad()
        {
            return await _context.LgConversionUnidad.ToListAsync();
        }

        // GET: api/LgConversionUnidads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgConversionUnidad>> GetLgConversionUnidad(int id)
        {
            var lgConversionUnidad = await _context.LgConversionUnidad.FindAsync(id);

            if (lgConversionUnidad == null)
            {
                return NotFound();
            }

            return lgConversionUnidad;
        }

        // PUT: api/LgConversionUnidads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgConversionUnidad(int id, LgConversionUnidad lgConversionUnidad)
        {
            if (id != lgConversionUnidad.ConversionId)
            {
                return BadRequest();
            }

            _context.Entry(lgConversionUnidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgConversionUnidadExists(id))
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

        // POST: api/LgConversionUnidads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgConversionUnidad>> PostLgConversionUnidad(LgConversionUnidad lgConversionUnidad)
        {
            _context.LgConversionUnidad.Add(lgConversionUnidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgConversionUnidad", new { id = lgConversionUnidad.ConversionId }, lgConversionUnidad);
        }

        // DELETE: api/LgConversionUnidads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgConversionUnidad(int id)
        {
            var lgConversionUnidad = await _context.LgConversionUnidad.FindAsync(id);
            if (lgConversionUnidad == null)
            {
                return NotFound();
            }

            _context.LgConversionUnidad.Remove(lgConversionUnidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgConversionUnidadExists(int id)
        {
            return _context.LgConversionUnidad.Any(e => e.ConversionId == id);
        }
    }
}
