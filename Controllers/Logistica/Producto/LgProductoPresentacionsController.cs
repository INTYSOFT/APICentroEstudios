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
    public class LgProductoPresentacionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgProductoPresentacionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgProductoPresentacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgProductoPresentacion>>> GetLgProductoPresentacion()
        {
            return await _context.LgProductoPresentacion.ToListAsync();
        }

        // GET: api/LgProductoPresentacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgProductoPresentacion>> GetLgProductoPresentacion(int id)
        {
            var lgProductoPresentacion = await _context.LgProductoPresentacion.FindAsync(id);

            if (lgProductoPresentacion == null)
            {
                return NotFound();
            }

            return lgProductoPresentacion;
        }

        // PUT: api/LgProductoPresentacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoPresentacion(int id, LgProductoPresentacion lgProductoPresentacion)
        {
            if (id != lgProductoPresentacion.ProductoPresentacionId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoPresentacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoPresentacionExists(id))
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

        // POST: api/LgProductoPresentacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgProductoPresentacion>> PostLgProductoPresentacion(LgProductoPresentacion lgProductoPresentacion)
        {
            _context.LgProductoPresentacion.Add(lgProductoPresentacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoPresentacion", new { id = lgProductoPresentacion.ProductoPresentacionId }, lgProductoPresentacion);
        }

        // DELETE: api/LgProductoPresentacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoPresentacion(int id)
        {
            var lgProductoPresentacion = await _context.LgProductoPresentacion.FindAsync(id);
            if (lgProductoPresentacion == null)
            {
                return NotFound();
            }

            _context.LgProductoPresentacion.Remove(lgProductoPresentacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoPresentacionExists(int id)
        {
            return _context.LgProductoPresentacion.Any(e => e.ProductoPresentacionId == id);
        }
    }
}
