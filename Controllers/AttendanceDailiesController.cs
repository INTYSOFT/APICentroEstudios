using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;

namespace api_intiSoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceDailiesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AttendanceDailiesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/AttendanceDailies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceDaily>>> GetAttendanceDaily()
        {
            return await _context.AttendanceDaily.ToListAsync();
        }

        // GET: api/AttendanceDailies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDaily>> GetAttendanceDaily(long id)
        {
            var attendanceDaily = await _context.AttendanceDaily.FindAsync(id);

            if (attendanceDaily == null)
            {
                return NotFound();
            }

            return attendanceDaily;
        }

        // PUT: api/AttendanceDailies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendanceDaily(long id, AttendanceDaily attendanceDaily)
        {
            if (id != attendanceDaily.Id)
            {
                return BadRequest();
            }

            _context.Entry(attendanceDaily).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceDailyExists(id))
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

        // POST: api/AttendanceDailies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AttendanceDaily>> PostAttendanceDaily(AttendanceDaily attendanceDaily)
        {
            _context.AttendanceDaily.Add(attendanceDaily);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendanceDaily", new { id = attendanceDaily.Id }, attendanceDaily);
        }

        // DELETE: api/AttendanceDailies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceDaily(long id)
        {
            var attendanceDaily = await _context.AttendanceDaily.FindAsync(id);
            if (attendanceDaily == null)
            {
                return NotFound();
            }

            _context.AttendanceDaily.Remove(attendanceDaily);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendanceDailyExists(long id)
        {
            return _context.AttendanceDaily.Any(e => e.Id == id);
        }
    }
}
