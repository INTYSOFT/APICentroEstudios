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
    public class ProductoVariantesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public ProductoVariantesController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LgProductoVariantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoVariante>>> GetLgProductoVariante()
        {
            return await _context.LgProductoVariante.ToListAsync();
        }

        // GET: api/LgProductoVariantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoVariante>> GetLgProductoVariante(int id)
        {
            var lgProductoVariante = await _context.LgProductoVariante.FindAsync(id);

            if (lgProductoVariante == null)
            {
                return NotFound();
            }

            return lgProductoVariante;
        }
        //Get por productoId
        [HttpGet("producto/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProductoVariante>>> GetLgProductoVarianteByProductoId(int id)
        {
            //DetalleListaFichaTecnicaId1
            var variantes = await _context.LgProductoVariante
               .AsNoTracking()
               .Include(v => v.VarianteDetalles)
                   .ThenInclude(d => d.DetalleListaFichaTecnica)
               .Where(v => v.ProductoId == id)
               .ToListAsync();

            return Ok(variantes); // ✅ SIEMPRE devuelve lista
        }


        //get con lista de detalles LgProductoVarianteDetalle
        [HttpGet("detalles/{id:int}")]
        public async Task<ActionResult<ProductoVariante>> GetLgProductoVarianteDetalles(int id)
        {
            var productoVariante = await _context.LgProductoVariante
                .AsNoTracking()
                .Include(v => v.VarianteDetalles)
                    .ThenInclude(d => d.DetalleListaFichaTecnica)
                .FirstOrDefaultAsync(v => v.ProductoVarianteId == id);

            return Ok(productoVariante ?? new ProductoVariante());
        }

        // PUT: api/LgProductoVariantes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutLgProductoVariante(int id, ProductoVariante lgProductoVariante)
        {
            if (id != lgProductoVariante.ProductoVarianteId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoVariante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoVarianteExists(id))
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

        // POST: api/LgProductoVariantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoVariante>> PostLgProductoVariante(ProductoVariante lgProductoVariante)
        {
            _context.LgProductoVariante.Add(lgProductoVariante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoVariante", new { id = lgProductoVariante.ProductoVarianteId }, lgProductoVariante);
        }

        // DELETE: api/LgProductoVariantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoVariante(int id)
        {
            var lgProductoVariante = await _context.LgProductoVariante.FindAsync(id);
            if (lgProductoVariante == null)
            {
                return NotFound();
            }

            _context.LgProductoVariante.Remove(lgProductoVariante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //post multiple LgProductoVarianteDto y sus detalles (LgProductoVarianteDetalleDto) 
        [HttpPost("postmultiplewithdetalle")]        
        public async Task<IActionResult> PostLgProductoVarianteMultipleConDetalle([FromBody] List<LgProductoVarianteDto> variantesDto)
        {
            if (variantesDto == null || !variantesDto.Any())
                return BadRequest("La lista no puede estar vacía.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var variantes = _mapper.Map<List<ProductoVariante>>(variantesDto);

                await _context.LgProductoVariante.AddRangeAsync(variantes);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                //retornar los ids de las variantes creadas List<LgProductoVarianteDto>
                var ids = variantes.Select(v => v.ProductoVarianteId).ToList();
                return Ok(ids);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //post multiple LgProductoVarianteDto
        [HttpPost("postmultiple")]
        public async Task<IActionResult> PostLgProductoVarianteMultiple([FromBody] List<Dto.Logistica.Producto.LgProductoVarianteDto> variantesDto)
        {
            if (variantesDto == null || !variantesDto.Any())
                return BadRequest("La lista no puede estar vacía.");

            var variantes = _mapper.Map<List<ProductoVariante>>(variantesDto);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.LgProductoVariante.AddRangeAsync(variantes);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(variantes.Select(v => v.ProductoVarianteId)); // O devuelve DTOs creados
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //_logger.LogError(ex, "Error guardando variantes.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        //put multiple LgProductoVarianteDto
        [HttpPut("putmultiple")]
        public async Task<IActionResult> PutLgProductoVarianteMultiple([FromBody] List<Dto.Logistica.Producto.LgProductoVarianteDto> variantesDto)
        {
            if (variantesDto == null || !variantesDto.Any())
                return BadRequest("La lista no puede estar vacía.");
            var variantes = _mapper.Map<List<ProductoVariante>>(variantesDto);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var variante in variantes)
                {
                    _context.Entry(variante).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    codigo = StatusCodes.Status200OK,
                    mensaje = "Fichas técnicas actualizadas correctamente.",
                    detalle = "Se han actualizado " + variantes.Count + " variantes."
                });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    codigo = StatusCodes.Status500InternalServerError,
                    mensaje = "Error al actualizar en la base de datos.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //_logger.LogError(ex, "Error guardando variantes.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        private bool LgProductoVarianteExists(int id)
        {
            return _context.LgProductoVariante.Any(e => e.ProductoVarianteId == id);
        }
    }
}
