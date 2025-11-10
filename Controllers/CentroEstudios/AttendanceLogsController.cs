using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLogsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AttendanceLogsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/AttendanceLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceLog>>> GetAttendanceLog()
        {
            return await _context.AttendanceLog.ToListAsync();
        }

        // GET: api/AttendanceLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceLog>> GetAttendanceLog(long id)
        {
            var attendanceLog = await _context.AttendanceLog.FindAsync(id);

            if (attendanceLog == null)
            {
                return NotFound();
            }

            return attendanceLog;
        }

        // PUT: api/AttendanceLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendanceLog(long id, AttendanceLog attendanceLog)
        {
            if (id != attendanceLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(attendanceLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceLogExists(id))
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

        // POST: api/AttendanceLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AttendanceLog>> PostAttendanceLog(AttendanceLog attendanceLog)
        {
            _context.AttendanceLog.Add(attendanceLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendanceLog", new { id = attendanceLog.Id }, attendanceLog);
        }

        // DELETE: api/AttendanceLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceLog(long id)
        {
            var attendanceLog = await _context.AttendanceLog.FindAsync(id);
            if (attendanceLog == null)
            {
                return NotFound();
            }

            _context.AttendanceLog.Remove(attendanceLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendanceLogExists(long id)
        {
            return _context.AttendanceLog.Any(e => e.Id == id);
        }
    }
}
