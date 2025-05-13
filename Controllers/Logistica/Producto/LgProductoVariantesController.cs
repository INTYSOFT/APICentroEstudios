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

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgProductoVariantesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public LgProductoVariantesController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LgProductoVariantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgProductoVariante>>> GetLgProductoVariante()
        {
            return await _context.LgProductoVariante.ToListAsync();
        }

        // GET: api/LgProductoVariantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgProductoVariante>> GetLgProductoVariante(int id)
        {
            var lgProductoVariante = await _context.LgProductoVariante.FindAsync(id);

            if (lgProductoVariante == null)
            {
                return NotFound();
            }

            return lgProductoVariante;
        }

        // PUT: api/LgProductoVariantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoVariante(int id, LgProductoVariante lgProductoVariante)
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
        public async Task<ActionResult<LgProductoVariante>> PostLgProductoVariante(LgProductoVariante lgProductoVariante)
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

        //post multiple 
        [HttpPost("multiple")]
        public async Task<IActionResult> PostLgProductoVarianteMultiple([FromBody] List<Dto.Logistica.Producto.LgProductoVarianteDto> variantesDto)
        {
            if (variantesDto == null || !variantesDto.Any())
                return BadRequest("La lista no puede estar vacía.");

            var variantes = _mapper.Map<List<LgProductoVariante>>(variantesDto);

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


        private bool LgProductoVarianteExists(int id)
        {
            return _context.LgProductoVariante.Any(e => e.ProductoVarianteId == id);
        }
    }
}
