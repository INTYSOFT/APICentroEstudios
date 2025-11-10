using api_intiSoft.DTO.CentroEstudios;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities; // <-- agrega este using arriba

namespace api_intiSoft.Controllers.CentroEstudios
{
    [ApiController]
    [Route("iclock")]
    public class IclockController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly ILogger<IclockController> _logger;

        public IclockController(ConecDinamicaContext context, ILogger<IclockController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("cdata")]
        [Produces("text/plain")]
        public IActionResult CDataOptions([FromQuery] string? SN = null)
        {
            _logger.LogInformation("ADMS cdata (GET/options) SN={SN} QS={QS}", SN, Request.QueryString.Value);
            // Responder solo "OK" habilita al equipo a continuar con el POST de datos.
            return Content("OK", "text/plain", Encoding.ASCII);
        }


        [HttpGet("getrequest")]
        [Produces("text/plain")]
        public IActionResult GetRequest([FromQuery] string? SN = null)
        {
            _logger.LogInformation("ADMS getrequest SN={SN} QS={QS}", SN, Request.QueryString.Value);
            return Content("OK", "text/plain", Encoding.ASCII);
        }

        // SIN [Consumes] ni [FromBody]/[FromForm] para evitar 415
        [HttpPost("cdata")]
        [Produces("text/plain")]
        public async Task<IActionResult> CData([FromQuery] string? SN = null)
        {
            var deviceSn = string.IsNullOrWhiteSpace(SN) ? "UNKNOWN" : SN.Trim();

            // 1) Lee body (texto plano) o form-urlencoded
            string rawBody = "";
            string? table = null;

            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync();
                table   = form.TryGetValue("table", out var tv) ? tv.ToString() : null;
                rawBody = form.TryGetValue("data", out var dv) ? dv.ToString() : "";
            }
            else
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                rawBody = await reader.ReadToEndAsync();

                // ⚠️ Soporte para "text/plain" con pares key=value (p.ej. table=ATTLOG&data=...)
                // Si viene así, interpretamos como query-string
                if (!string.IsNullOrWhiteSpace(rawBody) && rawBody.Contains('='))
                {
                    var qs = QueryHelpers.ParseQuery(rawBody);
                    if (qs.TryGetValue("table", out var tv)) table = tv.ToString();
                    if (qs.TryGetValue("data", out var dv))
                    {
                        // URL-decode por si viene con %0A, %20, etc.
                        rawBody = Uri.UnescapeDataString(dv.ToString());
                    }
                    else
                    {
                        // No hay 'data' => suele ser solo "Stamp=..." (sin registros nuevos)
                        // Responder OK para que el reloj siga con su ciclo.
                        Directory.CreateDirectory("logs");
                        var lf = Path.Combine("logs", $"iclock_{DateTime.UtcNow:yyyyMMdd}.log");
                        await System.IO.File.AppendAllTextAsync(lf,
                            $"[{DateTime.UtcNow:o}] SN={deviceSn} CT={Request.ContentType} (QS no-data)\n{rawBody}\n---\n");
                        _logger.LogInformation("cdata sin 'data'. SN={SN}, body='{Body}'", deviceSn, rawBody);
                        return Content("OK", "text/plain", Encoding.ASCII);
                    }
                }
            }

            rawBody ??= string.Empty;

            // 2) Log crudo SIEMPRE
            Directory.CreateDirectory("logs");
            var logFile = Path.Combine("logs", $"iclock_{DateTime.UtcNow:yyyyMMdd}.log");
            await System.IO.File.AppendAllTextAsync(logFile,
                $"[{DateTime.UtcNow:o}] SN={deviceSn} CT={Request.ContentType}\n{rawBody}\n---\n");

            if (!string.IsNullOrEmpty(table) &&
                !table.Equals("ATTLOG", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("cdata con table={table} distinto de ATTLOG; SN={SN}", table, deviceSn);
                return Content("OK", "text/plain", Encoding.ASCII);
            }

            // 3) Parseo → DTOs (ya convierten a UTC)
            var dtos = ParseToDtos(deviceSn, rawBody);
            if (dtos.Count == 0)
            {
                _logger.LogWarning("cdata SIN registros parseados. SN={SN}, len={Len}, CT={CT}, first100={F}",
                    deviceSn, rawBody.Length, Request.ContentType,
                    rawBody.Length > 100 ? rawBody[..100] : rawBody);
                return Content("OK", "text/plain", Encoding.ASCII);
            }

            // 4) Deduplicación por ventana
            var minUtc = dtos.Min(d => d.PunchTimeUtc);
            var maxUtc = dtos.Max(d => d.PunchTimeUtc);

            var existing = await _context.AttendanceLog
                .AsNoTracking()
                .Where(x => x.DeviceSn == deviceSn && x.PunchTime >= minUtc && x.PunchTime <= maxUtc)
                .Select(x => new { x.DeviceSn, x.UserId, x.PunchTime })
                .ToListAsync();

            var exists = new HashSet<(string sn, string uid, DateTime t)>(
                existing.Select(e => (e.DeviceSn, e.UserId, e.PunchTime)));

            var newDtos = dtos.Where(d => !exists.Contains((d.DeviceSn, d.UserId, d.PunchTimeUtc))).ToList();
            if (newDtos.Count == 0)
            {
                _logger.LogInformation("cdata OK. SN={SN}, nada nuevo (duplicados).", deviceSn);
                return Content("OK", "text/plain", Encoding.ASCII);
            }

            var entities = newDtos.Select(d => new AttendanceLog
            {
                DeviceSn  = d.DeviceSn,
                UserId    = d.UserId,
                PunchTime = d.PunchTimeUtc, // UTC
                Payload   = d.Payload,
                RawLine   = d.RawLine,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _context.AttendanceLog.AddRange(entities);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("cdata OK. SN={SN}, insertados={Count}", deviceSn, entities.Count);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "cdata SaveChanges con conflicto de duplicado; se ignoran los ya existentes.");
            }

            return Content("OK", "text/plain", Encoding.ASCII);
        }

        // -------- Helpers --------

        // Ajusta a la TZ donde está el reloj (Windows/IANA). Fallback: Local
        private static TimeZoneInfo GetDeviceTimeZone()
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"); } // Windows
            catch
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById("America/Lima"); }          // IANA
                catch { return TimeZoneInfo.Local; }
            }
        }

        private static bool TryParseDate(string s, out DateTime dt)
        {
            var formats = new[]
            {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss",
                "dd/MM/yyyy HH:mm:ss",
                "MM/dd/yyyy HH:mm:ss"
            };
            return DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dt)
                   || DateTime.TryParse(s, out dt);
        }

        // Recibe el cuerpo crudo y devuelve DTOs listos para guardar (PunchTime ya en UTC)
        private static List<AttendanceLogCreateDto> ParseToDtos(string deviceSn, string rawBody)
        {
            var deviceTz = GetDeviceTimeZone();
            var list = new List<AttendanceLogCreateDto>();
            var lines = rawBody.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var L in lines)
            {
                var line = L.Trim();
                if (line.Length == 0) continue;

                // A) "ATTLOG,YYYY-MM-DD HH:MM:SS,UserId,..."
                if (line.StartsWith("ATTLOG", StringComparison.OrdinalIgnoreCase))
                {
                    var rest = line.Substring(6).TrimStart(':', ',', ' ', '\t');
                    var parts = rest.Split(new[] { ',', '\t', ';' }, StringSplitOptions.None);
                    if (parts.Length >= 2 && TryParseDate(parts[0].Trim(), out var tsLocal)
                                          && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        var localUnspecified = DateTime.SpecifyKind(tsLocal, DateTimeKind.Unspecified);
                        var punchUtc = TimeZoneInfo.ConvertTimeToUtc(localUnspecified, deviceTz);

                        list.Add(new AttendanceLogCreateDto
                        {
                            DeviceSn = deviceSn,
                            UserId   = parts[1].Trim(),
                            PunchTimeUtc = punchUtc,
                            Payload  = parts.Length > 2 ? string.Join(",", parts.Skip(2)) : null,
                            RawLine  = line
                        });
                    }
                    continue;
                }

                // B) "PIN=1001  Time=YYYY-MM-DD HH:MM:SS ..."
                if (line.Contains("PIN=") && (line.Contains("Time=") || line.Contains("DateTime=")))
                {
                    var mPin = Regex.Match(line, @"PIN\s*=\s*([^\s\t,;]+)");
                    var mTime = Regex.Match(line, @"(?:Time|DateTime)\s*=\s*([0-9:\-\s/]+)");
                    if (mPin.Success && mTime.Success && TryParseDate(mTime.Groups[1].Value.Trim(), out var tsLocal))
                    {
                        var localUnspecified = DateTime.SpecifyKind(tsLocal, DateTimeKind.Unspecified);
                        var punchUtc = TimeZoneInfo.ConvertTimeToUtc(localUnspecified, deviceTz);

                        list.Add(new AttendanceLogCreateDto
                        {
                            DeviceSn = deviceSn,
                            UserId   = mPin.Groups[1].Value.Trim(),
                            PunchTimeUtc = punchUtc,
                            Payload  = line,
                            RawLine  = line
                        });
                    }
                    continue;
                }

                // C) CSV minimalista "YYYY-MM-DD HH:MM:SS,<UserId>[,...]"
                var csv = line.Split(',');
                if (csv.Length >= 2 && TryParseDate(csv[0].Trim(), out var ts2) && !string.IsNullOrWhiteSpace(csv[1]))
                {
                    var localUnspecified = DateTime.SpecifyKind(ts2, DateTimeKind.Unspecified);
                    var punchUtc = TimeZoneInfo.ConvertTimeToUtc(localUnspecified, deviceTz);

                    list.Add(new AttendanceLogCreateDto
                    {
                        DeviceSn = deviceSn,
                        UserId   = csv[1].Trim(),
                        PunchTimeUtc = punchUtc,
                        Payload  = csv.Length > 2 ? string.Join(",", csv.Skip(2)) : null,
                        RawLine  = line
                    });
                }

                // D) Formato con espacios/tabs: "<UserId> <YYYY-MM-DD HH:mm:ss> <otros campos...>"
                {
                    var m = Regex.Match(line, @"^\s*(?<uid>\S+)\s+(?<ts>\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2})(?<rest>.*)$");
                    if (m.Success && TryParseDate(m.Groups["ts"].Value.Trim(), out var tsLocal))
                    {
                        var uid = m.Groups["uid"].Value.Trim();
                        if (!string.IsNullOrWhiteSpace(uid))
                        {
                            // Normaliza el resto como payload legible
                            var rest = m.Groups["rest"].Value;
                            var normalized = Regex.Replace(rest, @"\s+", " ").Trim();

                            var localUnspecified = DateTime.SpecifyKind(tsLocal, DateTimeKind.Unspecified);
                            var punchUtc = TimeZoneInfo.ConvertTimeToUtc(localUnspecified, deviceTz);

                            list.Add(new AttendanceLogCreateDto
                            {
                                DeviceSn     = deviceSn,
                                UserId       = uid,
                                PunchTimeUtc = punchUtc,
                                Payload      = normalized,   // ej: "0 1 0 0 0 255 0 0"
                                RawLine      = line
                            });
                        }
                    }
                }


            }

            return list;
        }




[HttpGet("by-date")]
    public async Task<IActionResult> GetByDate(
    [FromQuery] DateTime? date,                    // yyyy-MM-dd (fecha LOCAL de referencia)
    [FromQuery] string? userId,
    [FromQuery] string? deviceSn,
    [FromQuery] string? tz = "SA Pacific Standard Time", // o "America/Lima"
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 1000)
    {
        if (date is null)
            return BadRequest("Parámetro 'date' requerido. Ej: /iclock/by-date?date=2025-11-09");

        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 5000) pageSize = 1000;

        // --- 1) Resolver TZ (Windows o IANA) ---
        static TimeZoneInfo ResolveTz(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) id = "SA Pacific Standard Time";
            try { return TimeZoneInfo.FindSystemTimeZoneById(id!); }
            catch
            {
                if (id!.Equals("SA Pacific Standard Time", StringComparison.OrdinalIgnoreCase))
                { try { return TimeZoneInfo.FindSystemTimeZoneById("America/Lima"); } catch { } }
                else if (id!.Equals("America/Lima", StringComparison.OrdinalIgnoreCase))
                { try { return TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"); } catch { } }
                return TimeZoneInfo.Local;
            }
        }
        var tzinfo = ResolveTz(tz);

        // --- 2) Ventana local [00:00, 24:00) -> UTC ---
        var startLocal = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0, DateTimeKind.Unspecified);
        var endLocal = startLocal.AddDays(1);
        var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tzinfo);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, tzinfo);

        // --- 3) Trae TODAS las marcaciones de la ventana (sin filtrar por user/device para el guardado) ---
        var allLogs = await _context.AttendanceLog.AsNoTracking()
            .Where(x => x.PunchTime >= startUtc && x.PunchTime < endUtc)
            .OrderBy(x => x.PunchTime)
            .ToListAsync();

        // Calcula LocalDate por cada punch (según TZ) y junta las fechas locales realmente presentes
        var nowUtc = DateTime.UtcNow;
        var rowsDaily = new List<AttendanceDaily>(allLogs.Count);
        var localDatesToRefresh = new HashSet<DateOnly>();

        foreach (var x in allLogs)
        {
            var ptUtc = DateTime.SpecifyKind(x.PunchTime, DateTimeKind.Utc);
            var ptLocal = TimeZoneInfo.ConvertTimeFromUtc(ptUtc, tzinfo);
            var localD = DateOnly.FromDateTime(ptLocal);

            localDatesToRefresh.Add(localD); // ← fechas a borrar/rehacer

            rowsDaily.Add(new AttendanceDaily
            {
                LocalDate      = localD,
                DeviceSn       = x.DeviceSn,
                UserId         = x.UserId,
                PunchTimeUtc   = ptUtc,
                PunchTimeLocal = DateTime.SpecifyKind(ptLocal, DateTimeKind.Unspecified),
                Payload        = x.Payload,
                RawLine        = x.RawLine,
                CreatedAt      = nowUtc
            });
        }

        // --- 4) REBUILD en attendance_daily (borra solo las fechas locales que aparecen y luego inserta todo) ---
        using (var tx = await _context.Database.BeginTransactionAsync())
        {
            foreach (var d in localDatesToRefresh)
            {
                var dText = new DateTime(d.Year, d.Month, d.Day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM academia.attendance_daily WHERE local_date = {0}::date;", dText);
            }

            if (rowsDaily.Count > 0)
            {
                _context.AttendanceDaily.AddRange(rowsDaily);
                await _context.SaveChangesAsync();
            }

            await tx.CommitAsync();
        }

        // --- 5) RESPUESTA: devolver items (con filtros opcionales) desde attendance_logs, como ya hacías ---
        var q = _context.AttendanceLog.AsNoTracking()
            .Where(x => x.PunchTime >= startUtc && x.PunchTime < endUtc);

        if (!string.IsNullOrWhiteSpace(userId))
            q = q.Where(x => x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(deviceSn))
            q = q.Where(x => x.DeviceSn == deviceSn);

        var total = await q.CountAsync();

        var rows = await q
            .OrderBy(x => x.PunchTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = rows.Select(x => new AttendanceLogViewDto
        {
            Id = x.Id,
            DeviceSn = x.DeviceSn,
            UserId = x.UserId,
            PunchTimeUtc = DateTime.SpecifyKind(x.PunchTime, DateTimeKind.Utc),
            PunchTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(x.PunchTime, DateTimeKind.Utc), tzinfo),
            Payload = x.Payload,
            RawLine = x.RawLine,
            CreatedAtUtc = DateTime.SpecifyKind(x.CreatedAt, DateTimeKind.Utc)
        }).ToList();

        return Ok(new
        {
            dateLocal = startLocal.ToString("yyyy-MM-dd"),
            tz = tzinfo.Id,
            windowUtc = new
            {
                startUtc = startUtc.ToString("yyyy-MM-dd HH:mm:ss"),
                endUtc = endUtc.ToString("yyyy-MM-dd HH:mm:ss")
            },
            rebuiltDaily = true,
            page,
            pageSize,
            total,
            items
        });
    }



    // DTO de salida
    public sealed class AttendanceLogViewDto
        {
            public long Id { get; init; }
            public string DeviceSn { get; init; } = default!;
            public string UserId { get; init; } = default!;
            public DateTime PunchTimeUtc { get; init; }      // guardado en UTC en BD
            public DateTime PunchTimeLocal { get; init; }    // convertido a la TZ solicitada
            public string? Payload { get; init; }
            public string RawLine { get; init; } = default!;
            public DateTime CreatedAtUtc { get; init; }
        }
    }
}
