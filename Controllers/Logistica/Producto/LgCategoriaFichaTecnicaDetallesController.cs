using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using Microsoft.AspNetCore.JsonPatch;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgCategoriaFichaTecnicaDetallesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgCategoriaFichaTecnicaDetallesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgCategoriaFichaTecnicaDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCategoriaFichaTecnicaDetalle>>> GetLgCategoriaFichaTecnicaDetalle()
        {
            return await _context.LgCategoriaFichaTecnicaDetalle.ToListAsync();
        }

        // GET: api/LgCategoriaFichaTecnicaDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCategoriaFichaTecnicaDetalle>> GetLgCategoriaFichaTecnicaDetalle(int id)
        {
            var lgCategoriaFichaTecnicaDetalle = await _context.LgCategoriaFichaTecnicaDetalle.FindAsync(id);

            if (lgCategoriaFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            return lgCategoriaFichaTecnicaDetalle;
        }

        // PUT: api/LgCategoriaFichaTecnicaDetalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgCategoriaFichaTecnicaDetalle(int id, LgCategoriaFichaTecnicaDetalle lgCategoriaFichaTecnicaDetalle)
        {
            if (id != lgCategoriaFichaTecnicaDetalle.CategoriaFichaTecnicaDetalleId)
            {
                return BadRequest();
            }

            _context.Entry(lgCategoriaFichaTecnicaDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgCategoriaFichaTecnicaDetalleExists(id))
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

        //getByCategoriaFichaTecnicaId
        [HttpGet("getByCategoriaFichaTecnicaId/{id}")]
        public async Task<ActionResult<IEnumerable<LgCategoriaFichaTecnicaDetalle>>> GetByCategoriaFichaTecnicaId(int id)
        {
            var lgCategoriaFichaTecnicaDetalle = await _context.LgCategoriaFichaTecnicaDetalle.Where(x => x.CategoriaFichaTecnicaId == id).ToListAsync();

            if (lgCategoriaFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            return lgCategoriaFichaTecnicaDetalle;
        }

        // POST: api/LgCategoriaFichaTecnicaDetalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCategoriaFichaTecnicaDetalle>> PostLgCategoriaFichaTecnicaDetalle(LgCategoriaFichaTecnicaDetalle lgCategoriaFichaTecnicaDetalle)
        {
            _context.LgCategoriaFichaTecnicaDetalle.Add(lgCategoriaFichaTecnicaDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgCategoriaFichaTecnicaDetalle", new { id = lgCategoriaFichaTecnicaDetalle.CategoriaFichaTecnicaDetalleId }, lgCategoriaFichaTecnicaDetalle);
        }

        //patch
        [HttpPatch("{id}")]
        public async Task<ActionResult<LgCategoriaFichaTecnicaDetalle>> Patch(int id, [FromBody] JsonPatchDocument<LgCategoriaFichaTecnicaDetalle> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }

            var lgCategoriaFichaTecnicaDetalle = await _context.LgCategoriaFichaTecnicaDetalle.FindAsync(id);

            if (lgCategoriaFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            patch.ApplyTo(lgCategoriaFichaTecnicaDetalle, ModelState);

            var isValid = TryValidateModel(lgCategoriaFichaTecnicaDetalle);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return lgCategoriaFichaTecnicaDetalle;
        }

        // DELETE: api/LgCategoriaFichaTecnicaDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgCategoriaFichaTecnicaDetalle(int id)
        {
            var lgCategoriaFichaTecnicaDetalle = await _context.LgCategoriaFichaTecnicaDetalle.FindAsync(id);
            if (lgCategoriaFichaTecnicaDetalle == null)
            {
                return NotFound();
            }

            _context.LgCategoriaFichaTecnicaDetalle.Remove(lgCategoriaFichaTecnicaDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgCategoriaFichaTecnicaDetalleExists(int id)
        {
            return _context.LgCategoriaFichaTecnicaDetalle.Any(e => e.CategoriaFichaTecnicaDetalleId == id);
        }
    }
}
