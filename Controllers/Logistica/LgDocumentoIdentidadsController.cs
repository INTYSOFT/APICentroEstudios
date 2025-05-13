using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgDocumentoIdentidadsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgDocumentoIdentidadsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgDocumentoIdentidads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgDocumentoIdentidad>>> GetLgDocumentoIdentidad()
        {
            return await _context.LgDocumentoIdentidad.ToListAsync();
        }

        // GET: api/LgDocumentoIdentidads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgDocumentoIdentidad>> GetLgDocumentoIdentidad(string id)
        {
            var lgDocumentoIdentidad = await _context.LgDocumentoIdentidad.FindAsync(id);

            if (lgDocumentoIdentidad == null)
            {
                return NotFound();
            }

            return lgDocumentoIdentidad;
        }

        // PUT: api/LgDocumentoIdentidads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgDocumentoIdentidad(string id, LgDocumentoIdentidad lgDocumentoIdentidad)
        {
            if (id != lgDocumentoIdentidad.DocumentoIdentidadId)
            {
                return BadRequest();
            }

            _context.Entry(lgDocumentoIdentidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgDocumentoIdentidadExists(id))
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

        // POST: api/LgDocumentoIdentidads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgDocumentoIdentidad>> PostLgDocumentoIdentidad(LgDocumentoIdentidad lgDocumentoIdentidad)
        {
            _context.LgDocumentoIdentidad.Add(lgDocumentoIdentidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LgDocumentoIdentidadExists(lgDocumentoIdentidad.DocumentoIdentidadId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLgDocumentoIdentidad", new { id = lgDocumentoIdentidad.DocumentoIdentidadId }, lgDocumentoIdentidad);
        }

        // DELETE: api/LgDocumentoIdentidads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgDocumentoIdentidad(string id)
        {
            var lgDocumentoIdentidad = await _context.LgDocumentoIdentidad.FindAsync(id);
            if (lgDocumentoIdentidad == null)
            {
                return NotFound();
            }

            _context.LgDocumentoIdentidad.Remove(lgDocumentoIdentidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgDocumentoIdentidadExists(string id)
        {
            return _context.LgDocumentoIdentidad.Any(e => e.DocumentoIdentidadId == id);
        }
    }
}
