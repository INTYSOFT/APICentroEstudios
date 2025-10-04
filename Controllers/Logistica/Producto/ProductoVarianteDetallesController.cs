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
using System.ComponentModel.DataAnnotations.Schema;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVarianteDetallesController : ControllerBase
    {
        public record VariantesDetalleRequest(
            int ProductoId,
            bool SoloActivos = true
        );

        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public ProductoVarianteDetallesController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LgProductoVarianteDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoVarianteDetalle>>> GetLgProductoVarianteDetalle()
        {
            return await _context.LgProductoVarianteDetalle.ToListAsync();
        }

        // GET: api/LgProductoVarianteDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoVarianteDetalle>> GetLgProductoVarianteDetalle(int id)
        {
            var lgProductoVarianteDetalle = await _context.LgProductoVarianteDetalle.FindAsync(id);

            if (lgProductoVarianteDetalle == null)
            {
                return NotFound();
            }

            return lgProductoVarianteDetalle;
        }



        //get por productoId. ten en cuenta la relación  //[ForeignKey         [ForeignKey("ProductoVarianteId")]         [InverseProperty("VarianteDetalles")]        public virtual LgProductoVariante ProductoVariante { get; set; } = null!;
        [HttpPost("byProducto")]
        public async Task<ActionResult<IEnumerable<VarianteDetalleJoinDto>>> PostByProducto([FromBody] VariantesDetalleRequest req)
        {
            if (req is null || req.ProductoId <= 0)
                return BadRequest("ProductoId inválido.");

            var query =
                from v in _context.Set<ProductoVariante>().AsNoTracking()
                join d in _context.Set<ProductoVarianteDetalle>().AsNoTracking()
                    on v.ProductoVarianteId equals d.ProductoVarianteId
                where v.ProductoId == req.ProductoId
                   && (!req.SoloActivos || v.Activo == true)
                   && (!req.SoloActivos || d.Activo == true)
                orderby v.Nombre, d.Dato
                select new VarianteDetalleJoinDto(
                    v.ListaFichaTecnicaId,
                    v.Nombre,
                    d.ProductoVarianteDetalleId,
                    d.ProductoVarianteId,
                    d.DetalleListaFichaTecnicaId,
                    d.Dato,
                    d.Descripcion,
                    d.Activo
                );

            var result = await query.ToListAsync();
            return Ok(result);
        }



        // PUT: api/LgProductoVarianteDetalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoVarianteDetalle(int id, ProductoVarianteDetalle lgProductoVarianteDetalle)
        {
            if (id != lgProductoVarianteDetalle.ProductoVarianteDetalleId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoVarianteDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoVarianteDetalleExists(id))
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

        // POST: api/LgProductoVarianteDetalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoVarianteDetalle>> PostLgProductoVarianteDetalle(ProductoVarianteDetalle lgProductoVarianteDetalle)
        {
            _context.LgProductoVarianteDetalle.Add(lgProductoVarianteDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoVarianteDetalle", new { id = lgProductoVarianteDetalle.ProductoVarianteDetalleId }, lgProductoVarianteDetalle);
        }

        // DELETE: api/LgProductoVarianteDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoVarianteDetalle(int id)
        {
            var lgProductoVarianteDetalle = await _context.LgProductoVarianteDetalle.FindAsync(id);
            if (lgProductoVarianteDetalle == null)
            {
                return NotFound();
            }

            _context.LgProductoVarianteDetalle.Remove(lgProductoVarianteDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //post multiple LgProductoVarianteDetalleDto
        [HttpPost("postmultiple")]
        public async Task<IActionResult> PostLgProductoVarianteDetalleMultiple([FromBody] List<Dto.Logistica.Producto.LgProductoVarianteDetalleDto> detallesDto)
        {
            if (detallesDto == null || !detallesDto.Any())
                return BadRequest("La lista no puede estar vacía.");
            var detalles = _mapper.Map<List<ProductoVarianteDetalle>>(detallesDto);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.LgProductoVarianteDetalle.AddRangeAsync(detalles);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(detalles.Select(d => d.ProductoVarianteDetalleId));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        //put multiple
        [HttpPut("putmultiple")]
        public async Task<IActionResult> PutLgProductoVarianteDetalleMultiple([FromBody] List<Dto.Logistica.Producto.LgProductoVarianteDetalleDto> detallesDto)
        {
            if (detallesDto == null || !detallesDto.Any())
                return BadRequest("La lista no puede estar vacía.");
            var detalles = _mapper.Map<List<ProductoVarianteDetalle>>(detallesDto);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var detalle in detalles)
                {
                    _context.Entry(detalle).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(detalles.Select(d => d.ProductoVarianteDetalleId));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        private bool LgProductoVarianteDetalleExists(int id)
        {
            return _context.LgProductoVarianteDetalle.Any(e => e.ProductoVarianteDetalleId == id);
        }
    }
}
