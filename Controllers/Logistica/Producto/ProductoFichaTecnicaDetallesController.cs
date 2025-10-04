using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoFichaTecnicaDetallesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ProductoFichaTecnicaDetallesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgProductoFichaTecnicaDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoFichaTecnicaDetalle>>> GetLgProductoFichaTecnicaDetalle()
        {
            return await _context.LgProductoFichaTecnicaDetalle.ToListAsync();
        }

        // GET: api/LgProductoFichaTecnicaDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoFichaTecnicaDetalle>> GetLgProductoFichaTecnicaDetalle(int id)
        {
            var lgProductoFichaTecnicaDetalle = await _context.LgProductoFichaTecnicaDetalle.FindAsync(id);

            if (lgProductoFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            return lgProductoFichaTecnicaDetalle;
        }

        // PUT: api/LgProductoFichaTecnicaDetalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoFichaTecnicaDetalle(int id, ProductoFichaTecnicaDetalle lgProductoFichaTecnicaDetalle)
        {
            if (id != lgProductoFichaTecnicaDetalle.ProductoFichaTecnicaDetalleId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoFichaTecnicaDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoFichaTecnicaDetalleExists(id))
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

        // POST: api/LgProductoFichaTecnicaDetalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoFichaTecnicaDetalle>> PostLgProductoFichaTecnicaDetalle(ProductoFichaTecnicaDetalle lgProductoFichaTecnicaDetalle)
        {
            _context.LgProductoFichaTecnicaDetalle.Add(lgProductoFichaTecnicaDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoFichaTecnicaDetalle", new { id = lgProductoFichaTecnicaDetalle.ProductoFichaTecnicaDetalleId }, lgProductoFichaTecnicaDetalle);
        }

        // DELETE: api/LgProductoFichaTecnicaDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoFichaTecnicaDetalle(int id)
        {
            var lgProductoFichaTecnicaDetalle = await _context.LgProductoFichaTecnicaDetalle.FindAsync(id);
            if (lgProductoFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            _context.LgProductoFichaTecnicaDetalle.Remove(lgProductoFichaTecnicaDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //POST MULTIPLE con LgProductoFichaTecnicaDetalleDto
        [HttpPost("postmultiple")]
        public async Task<IActionResult> PostLgProductoFichaTecnicaDetalleMultiple([FromBody] List<LgProductoFichaTecnicaDetalleDto> detalles)
        {
            if (detalles == null || !detalles.Any())
                return BadRequest("La lista no puede estar vacía.");
            var detallesEntity = detalles.Select(d => new ProductoFichaTecnicaDetalle
            {
                ProductoFichaTecnicaDetalleId = d.ProductoFichaTecnicaDetalleId,
                ProductoFichaTecnicaId = d.ProductoFichaTecnicaId,
                DetalleListaFichaTecnicaId = d.DetalleListaFichaTecnicaId,
                Dato = d.Dato,
                Descripcion = d.Descripcion,
                Activo = d.Activo
            }).ToList();
            _context.LgProductoFichaTecnicaDetalle.AddRange(detallesEntity);
            await _context.SaveChangesAsync();
            return Ok(detallesEntity);

        }

        //put multiple con LgProductoFichaTecnicaDetalleDto
        [HttpPut("putmultiple")]
        public async Task<IActionResult> PutLgProductoFichaTecnicaDetalleMultiple([FromBody] List<LgProductoFichaTecnicaDetalleDto> detalles)
        {
            if (detalles == null || !detalles.Any())
                return BadRequest("La lista no puede estar vacía.");
            var detallesEntity = detalles.Select(d => new ProductoFichaTecnicaDetalle
            {
                ProductoFichaTecnicaDetalleId = d.ProductoFichaTecnicaDetalleId,
                ProductoFichaTecnicaId = d.ProductoFichaTecnicaId,
                DetalleListaFichaTecnicaId = d.DetalleListaFichaTecnicaId,
                Dato = d.Dato,
                Descripcion = d.Descripcion,
                Activo = d.Activo
            }).ToList();
            _context.LgProductoFichaTecnicaDetalle.UpdateRange(detallesEntity);
            await _context.SaveChangesAsync();
            return Ok(detallesEntity);
        }


        private bool LgProductoFichaTecnicaDetalleExists(int id)
        {
            return _context.LgProductoFichaTecnicaDetalle.Any(e => e.ProductoFichaTecnicaDetalleId == id);
        }
    }
}
