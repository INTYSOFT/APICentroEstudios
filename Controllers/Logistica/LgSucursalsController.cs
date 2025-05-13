using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgSucursalsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgSucursalsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgSucursals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgSucursal>>> GetLgSucursal()
        {
            return await _context.LgSucursal.ToListAsync();
        }

        // GET: api/LgSucursals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgSucursal>> GetLgSucursal(int id)
        {
            var lgSucursal = await _context.LgSucursal.FindAsync(id);

            if (lgSucursal == null)
            {
                return NotFound();
            }

            return lgSucursal;
        }

        // PUT: api/LgSucursals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgSucursal(int id, LgSucursal lgSucursal)
        {
            if (id != lgSucursal.SucursalId)
            {
                return BadRequest();
            }

            _context.Entry(lgSucursal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgSucursalExists(id))
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

        // POST: api/LgSucursals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgSucursal>> PostLgSucursal(LgSucursal lgSucursal)
        {
            _context.LgSucursal.Add(lgSucursal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgSucursal", new { id = lgSucursal.SucursalId }, lgSucursal);
        }

        // DELETE: api/LgSucursals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgSucursal(int id)
        {
            var lgSucursal = await _context.LgSucursal.FindAsync(id);
            if (lgSucursal == null)
            {
                return NotFound();
            }

            _context.LgSucursal.Remove(lgSucursal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgSucursalExists(int id)
        {
            return _context.LgSucursal.Any(e => e.SucursalId == id);
        }
    }
}
