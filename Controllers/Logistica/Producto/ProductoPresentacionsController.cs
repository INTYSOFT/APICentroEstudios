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
    public class ProductoPresentacionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public ProductoPresentacionsController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LgProductoPresentacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoPresentacion>>> GetLgProductoPresentacion()
        {
            return await _context.LgProductoPresentacion.ToListAsync();
        }

        // GET: api/LgProductoPresentacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoPresentacion>> GetLgProductoPresentacion(int id)
        {
            var lgProductoPresentacion = await _context.LgProductoPresentacion.FindAsync(id);

            if (lgProductoPresentacion == null)
            {
                return NotFound();
            }

            return lgProductoPresentacion;
        }

        //Get por ProductoId.
        [HttpGet("byProducto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ProductoPresentacion>>> GetLgProductoPresentacionByProducto(int productoId)
        {
            var presentaciones = await _context.LgProductoPresentacion
                .Where(p => p.ProductoId == productoId)
                .OrderBy(p => p.NumeracionGrupo)
                .ThenBy(p => p.Orden)
                .ToListAsync();
            if (presentaciones == null || !presentaciones.Any())
            {
                return NotFound();
            }
            return presentaciones;
        }



        // PUT: api/LgProductoPresentacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoPresentacion(int id, ProductoPresentacion lgProductoPresentacion)
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
        public async Task<ActionResult<ProductoPresentacion>> PostLgProductoPresentacion(ProductoPresentacion lgProductoPresentacion)
        {
            _context.LgProductoPresentacion.Add(lgProductoPresentacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoPresentacion", new { id = lgProductoPresentacion.ProductoPresentacionId }, lgProductoPresentacion);
        }

        //POST miultiple
        [HttpPost("multiple")]
        public async Task<IActionResult> PostLgProductoPresentacionMultiple([FromBody] List<LgProductoPresentacionDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest("Debe proporcionar al menos una presentación de producto.");

            try
            {
                var entidades = _mapper.Map<List<ProductoPresentacion>>(dtos);

                await using var transaction = await _context.Database.BeginTransactionAsync();

                await _context.LgProductoPresentacion.AddRangeAsync(entidades);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Presentaciones creadas correctamente.",
                    ids = entidades.Select(e => e.ProductoPresentacionId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                // _logger.LogError(dbEx, "Error al guardar presentaciones.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al insertar presentaciones.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error inesperado al guardar presentaciones.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error interno al procesar la solicitud.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        //Put multiple
        [HttpPut("multiple")]
        public async Task<IActionResult> PutLgProductoPresentacionMultiple([FromBody] List<LgProductoPresentacionDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest("Debe proporcionar al menos una presentación de producto.");
            try
            {
                var entidades = _mapper.Map<List<ProductoPresentacion>>(dtos);
                await using var transaction = await _context.Database.BeginTransactionAsync();
                foreach (var entidad in entidades)
                {
                    _context.Entry(entidad).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    mensaje = "Presentaciones actualizadas correctamente.",
                    ids = entidades.Select(e => e.ProductoPresentacionId)
                });
            }
            catch (DbUpdateException dbEx)
            {
                // _logger.LogError(dbEx, "Error al actualizar presentaciones.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error de base de datos al actualizar presentaciones.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error inesperado al actualizar presentaciones.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error interno al procesar la solicitud.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
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
