using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsoProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public TipoUsoProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoUsoProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsoProducto>>> GetLgTipoUsoProducto()
        {
            return await _context.LgTipoUsoProducto.ToListAsync();
        }

        // GET: api/LgTipoUsoProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsoProducto>> GetLgTipoUsoProducto(short id)
        {
            var lgTipoUsoProducto = await _context.LgTipoUsoProducto.FindAsync(id);

            if (lgTipoUsoProducto == null)
            {
                return NotFound();
            }

            return lgTipoUsoProducto;
        }

        // PUT: api/LgTipoUsoProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoUsoProducto(short id, TipoUsoProducto lgTipoUsoProducto)
        {
            if (id != lgTipoUsoProducto.TipoUsoProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoUsoProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoUsoProductoExists(id))
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

        // POST: api/LgTipoUsoProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoUsoProducto>> PostLgTipoUsoProducto(TipoUsoProducto lgTipoUsoProducto)
        {
            _context.LgTipoUsoProducto.Add(lgTipoUsoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoUsoProducto", new { id = lgTipoUsoProducto.TipoUsoProductoId }, lgTipoUsoProducto);
        }

        private bool LgTipoUsoProductoExists(short id)
        {
            return _context.LgTipoUsoProducto.Any(e => e.TipoUsoProductoId == id);
        }
    }
}
