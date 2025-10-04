using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using AutoMapper;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVariantePresentacionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public ProductoVariantePresentacionsController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper=mapper;
        }

        // GET: api/LgProductoVariantePresentacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoVariantePresentacion>>> GetLgProductoVariantePresentacion()
        {
            return await _context.ProductoVariantePresentacion.ToListAsync();
        }

        // GET: api/LgProductoVariantePresentacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoVariantePresentacion>> GetLgProductoVariantePresentacion(int id)
        {
            var lgProductoVariantePresentacion = await _context.ProductoVariantePresentacion.FindAsync(id);

            if (lgProductoVariantePresentacion == null)
            {
                return NotFound();
            }

            return lgProductoVariantePresentacion;
        }

        //get por producto
        [HttpGet("byProducto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ProductoVariantePresentacion>>> GetLgProductoVariantePresentacionByProducto(int productoId)
        {
            var presentaciones = await _context.ProductoVariantePresentacion
                .Where(p => p.ProductoId == productoId)
                .ToListAsync();
            if (presentaciones == null || !presentaciones.Any())
            {
                return NotFound();
            }
            return presentaciones;
        }

        // PUT: api/LgProductoVariantePresentacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoVariantePresentacion(int id, ProductoVariantePresentacion lgProductoVariantePresentacion)
        {
            if (id != lgProductoVariantePresentacion.ProductoVariantePresentacionId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoVariantePresentacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoVariantePresentacionExists(id))
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

        // POST: api/LgProductoVariantePresentacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoVariantePresentacion>> PostLgProductoVariantePresentacion(ProductoVariantePresentacion lgProductoVariantePresentacion)
        {
            _context.ProductoVariantePresentacion.Add(lgProductoVariantePresentacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoVariantePresentacion", new { id = lgProductoVariantePresentacion.ProductoVariantePresentacionId }, lgProductoVariantePresentacion);
        }

        // POST: api/LgProductoVariantePresentacions/multiple
        [HttpPost("multiple")]
        public async Task<IActionResult> PostLgProductoVariantePresentacionMultiple([FromBody] List<LgProductoVariantePresentacionDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest("Debe proporcionar al menos una variante de presentación de producto.");

            try
            {
                // Mapear DTOs a entidades
                var entidades = _mapper.Map<List<ProductoVariantePresentacion>>(dtos);

                await using var transaction = await _context.Database.BeginTransactionAsync();

                await _context.ProductoVariantePresentacion.AddRangeAsync(entidades);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Variantes de presentación creadas correctamente.",
                    ids = entidades.Select(e => e.ProductoVariantePresentacionId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al crear variantes de presentación.",
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




        // PUT: api/LgProductoVariantePresentacions/multiple
        [HttpPut("multiple")]
        public async Task<IActionResult> PutLgProductoVariantePresentacionMultiple([FromBody] List<ProductoVariantePresentacion> entidades)
        {
            if (entidades == null || !entidades.Any())
                return BadRequest("Debe proporcionar al menos una variante de presentación de producto.");
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                foreach (var entidad in entidades)
                {
                    _context.Entry(entidad).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    mensaje = "Variantes de presentación actualizadas correctamente.",
                    ids = entidades.Select(e => e.ProductoVariantePresentacionId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al actualizar variantes de presentación.",
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

        // DELETE: api/LgProductoVariantePresentacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoVariantePresentacion(int id)
        {
            var lgProductoVariantePresentacion = await _context.ProductoVariantePresentacion.FindAsync(id);
            if (lgProductoVariantePresentacion == null)
            {
                return NotFound();
            }

            _context.ProductoVariantePresentacion.Remove(lgProductoVariantePresentacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoVariantePresentacionExists(int id)
        {
            return _context.ProductoVariantePresentacion.Any(e => e.ProductoVariantePresentacionId == id);
        }
    }
}
