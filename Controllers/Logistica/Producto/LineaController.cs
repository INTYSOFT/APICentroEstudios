using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    //class LgLinea Controller: ControllerBase
    public class LineaController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        public LineaController(ConecDinamicaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Linea>>> GetLgLinea()
        {
            return await _context.LgLinea.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Linea>> GetLgLinea(int id)
        {
            var LgLinea = await _context.LgLinea.FindAsync(id);
            if (LgLinea == null)
            {
                return NotFound();
            }
            return LgLinea;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgLinea(int id, Linea LgLinea)
        {
            if (id != LgLinea.LineaId)
            {
                return BadRequest();
            }
            _context.Entry(LgLinea).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgLineaExists(id))
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
        public async Task<ActionResult<Linea>> PostLgLinea(Linea LgLinea)
        {
            _context.LgLinea.Add(LgLinea);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLgLinea", new { id = LgLinea.LineaId }, LgLinea);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Linea>> DeleteLgLinea(int id)
        {
            var LgLinea = await _context.LgLinea.FindAsync(id);
            if (LgLinea == null)
            {
                return NotFound();
            }
            _context.LgLinea.Remove(LgLinea);
            await _context.SaveChangesAsync();
            return LgLinea;
        }
        private bool LgLineaExists(int id)
        {
            return _context.LgLinea.Any(e => e.LineaId == id);
        }



    }


}
