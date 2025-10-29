using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    //class LgMarca Controller: ControllerBase
    public class LgMarcasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        public LgMarcasController(ConecDinamicaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgMarca>>> GetLgMarca()
        {
            return await _context.LgMarca.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LgMarca>> GetLgMarca(int id)
        {
            var lgMarca = await _context.LgMarca.FindAsync(id);
            if (lgMarca == null)
            {
                return NotFound();
            }
            return lgMarca;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgMarca(int id, LgMarca lgMarca)
        {
            if (id != lgMarca.MarcaId)
            {
                return BadRequest();
            }
            _context.Entry(lgMarca).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgMarcaExists(id))
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
        [HttpPost]
        public async Task<ActionResult<LgMarca>> PostLgMarca(LgMarca lgMarca)
        {
            _context.LgMarca.Add(lgMarca);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLgMarca", new { id = lgMarca.MarcaId }, lgMarca);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LgMarca>> DeleteLgMarca(int id)
        {
            var lgMarca = await _context.LgMarca.FindAsync(id);
            if (lgMarca == null)
            {
                return NotFound();
            }
            _context.LgMarca.Remove(lgMarca);
            await _context.SaveChangesAsync();
            return lgMarca;
        }
        private bool LgMarcaExists(int id)
        {
            return _context.LgMarca.Any(e => e.MarcaId == id);
        }



    }


}
