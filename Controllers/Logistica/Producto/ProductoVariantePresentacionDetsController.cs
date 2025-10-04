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
    public class ProductoVariantePresentacionDetsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        public record PvpIdsReq(int[] PvpIds);

        public ProductoVariantePresentacionDetsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgProductoVariantePresentacionDets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoVariantePresentacionDet>>> GetLgProductoVariantePresentacionDet()
        {
            return await _context.LgProductoVariantePresentacionDet.ToListAsync();
        }

        // GET: api/LgProductoVariantePresentacionDets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoVariantePresentacionDet>> GetLgProductoVariantePresentacionDet(int id)
        {
            var lgProductoVariantePresentacionDet = await _context.LgProductoVariantePresentacionDet.FindAsync(id);

            if (lgProductoVariantePresentacionDet == null)
            {
                return NotFound();
            }

            return lgProductoVariantePresentacionDet;
        }

        // GET: api/LgProductoVariantePresentacionDets/por-producto/62
        [HttpGet("byproducto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ProductoVariantePresentacionDetDTO>>> GetDetallesPorProducto(int productoId)
        {
            var detalles = await _context.LgProductoVariantePresentacionDet
                .Where(d => d.ProductoVariantePresentacion.ProductoId == productoId)
                .Select(d => new ProductoVariantePresentacionDetDTO
                {
                    ProductoPresentacionId = d.ProductoVariantePresentacion.ProductoPresentacionId,
                    Detalle = d
                })
                .ToListAsync();

            return Ok(detalles);
        }



        //byPvpIds
        [HttpPost("byPvpIds")]
        public async Task<ActionResult<IEnumerable<ProductoVariantePresentacionDet>>> GetByPvpIds([FromBody] PvpIdsReq req)
        {
            if (req?.PvpIds == null || req.PvpIds.Length == 0)
                return BadRequest("Debe enviar al menos un PVP id.");

            var list = await _context.LgProductoVariantePresentacionDet
                .AsNoTracking()
                .Where(d => req.PvpIds.Contains(d.ProductoVariantePresentacionId))
                .ToListAsync();

            return Ok(list);
        }

        // PUT: api/LgProductoVariantePresentacionDets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoVariantePresentacionDet(int id, ProductoVariantePresentacionDet lgProductoVariantePresentacionDet)
        {
            if (id != lgProductoVariantePresentacionDet.ProductoVariantePresentacionDetId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoVariantePresentacionDet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoVariantePresentacionDetExists(id))
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

        // POST: api/LgProductoVariantePresentacionDets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoVariantePresentacionDet>> PostLgProductoVariantePresentacionDet(ProductoVariantePresentacionDet lgProductoVariantePresentacionDet)
        {
            _context.LgProductoVariantePresentacionDet.Add(lgProductoVariantePresentacionDet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoVariantePresentacionDet", new { id = lgProductoVariantePresentacionDet.ProductoVariantePresentacionDetId }, lgProductoVariantePresentacionDet);
        }

        /*Post Multiple*/
        [HttpPost]
        [Route("multiple")]
        public async Task<IActionResult> PostLgProductoVariantePresentacionDetMultiple([FromBody] List<ProductoVariantePresentacionDet> detalles)
        {
            if (detalles == null || !detalles.Any())
            {
                return BadRequest("Debe proporcionar al menos un detalle de variante de presentación de producto.");
            }
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                await _context.LgProductoVariantePresentacionDet.AddRangeAsync(detalles);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    mensaje = "Detalles de variantes de presentación creados correctamente.",
                    ids = detalles.Select(d => d.ProductoVariantePresentacionDetId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al crear detalles de variantes de presentación.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error interno al procesar la solicitud.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /*put multiple
        */
        [HttpPut]
        [Route("multiple")]
        public async Task<IActionResult> PutLgProductoVariantePresentacionDetMultiple([FromBody] List<ProductoVariantePresentacionDet> detalles)
        {
            if (detalles == null || !detalles.Any())
            {
                return BadRequest("Debe proporcionar al menos un detalle de variante de presentación de producto.");
            }
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                foreach (var detalle in detalles)
                {
                    _context.Entry(detalle).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    mensaje = "Detalles de variantes de presentación actualizados correctamente.",
                    ids = detalles.Select(d => d.ProductoVariantePresentacionDetId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al actualizar detalles de variantes de presentación.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error interno al procesar la solicitud.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
        }





        // DELETE: api/LgProductoVariantePresentacionDets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoVariantePresentacionDet(int id)
        {
            var lgProductoVariantePresentacionDet = await _context.LgProductoVariantePresentacionDet.FindAsync(id);
            if (lgProductoVariantePresentacionDet == null)
            {
                return NotFound();
            }

            _context.LgProductoVariantePresentacionDet.Remove(lgProductoVariantePresentacionDet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoVariantePresentacionDetExists(int id)
        {
            return _context.LgProductoVariantePresentacionDet.Any(e => e.ProductoVariantePresentacionDetId == id);
        }
    }
}
