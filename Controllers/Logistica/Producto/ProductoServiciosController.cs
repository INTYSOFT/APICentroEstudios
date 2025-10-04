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
    public class ProductoServiciosController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ProductoServiciosController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgProductoServicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoServicio>>> GetLgProductoServicio()
        {
            return await _context.LgProductoServicio.ToListAsync();
        }

        // GET: api/LgProductoServicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoServicio>> GetLgProductoServicio(int id)
        {
            var lgProductoServicio = await _context.LgProductoServicio.FindAsync(id);

            if (lgProductoServicio == null)
            {
                return NotFound();
            }

            return lgProductoServicio;
        }

        // PUT: api/LgProductoServicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoServicio(int id, ProductoServicio lgProductoServicio)
        {
            if (id != lgProductoServicio.ProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoServicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoServicioExists(id))
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

        // POST: api/LgProductoServicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoServicio>> PostLgProductoServicio(ProductoServicio lgProductoServicio)
        {
            _context.LgProductoServicio.Add(lgProductoServicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoServicio", new { id = lgProductoServicio.ProductoId }, lgProductoServicio);
        }

        // DELETE: api/LgProductoServicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoServicio(int id)
        {
            var lgProductoServicio = await _context.LgProductoServicio.FindAsync(id);
            if (lgProductoServicio == null)
            {
                return NotFound();
            }

            _context.LgProductoServicio.Remove(lgProductoServicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoServicioExists(int id)
        {
            return _context.LgProductoServicio.Any(e => e.ProductoId == id);
        }
    }
}
