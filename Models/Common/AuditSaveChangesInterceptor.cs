using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Npgsql;

namespace api_intiSoft.Models.Common;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private int? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var id) ? id : null;
    }

    private string? GetIpAddress() =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

    private string? GetUserAgent() =>
        _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();

    private string? GetModule() =>
        _httpContextAccessor.HttpContext?.Request?.Path.Value;

    private Guid GetTransactionId() =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.TraceIdentifier, out var id) ? id : Guid.NewGuid();

    private async Task ApplyAuditAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) return;

        var userId = GetCurrentUserId();
        var fecha = DateTime.UtcNow;

        // 1. Auditar campos básicos de entidades AuditableEntity
        var auditableEntries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in auditableEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaRegistro = fecha;
                entry.Entity.UsuaraioRegistroId ??= userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                // No modificar campos de registro, datos inmutables.
                entry.Property(p => p.FechaRegistro).IsModified = false;
                entry.Property(p => p.UsuaraioRegistroId).IsModified = false;

                // Actualizar campos de modificación, última modificación.
                entry.Entity.FechaActualizacion = fecha;
                entry.Entity.UsuaraioActualizacionId = userId;
            }
        }

        // 2. Guardar en log_auditoria solo si implementa IAuditableWithLog
        var entriesWithLog = auditableEntries
            .Where(e => e.Entity is IAuditableWithLog &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            .ToList();

        if (!entriesWithLog.Any()) return;

        var ip = GetIpAddress();
        var userAgent = GetUserAgent();
        var modulo = GetModule();
        var transaccionId = GetTransactionId();

        foreach (var entry in entriesWithLog)
        {
            var datosAntes = entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                : null;

            var datosDespues = entry.State == EntityState.Deleted
                ? null
                : JsonConvert.SerializeObject(entry.CurrentValues.ToObject());

            var entidadId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "";

            var sql = @$"
                INSERT INTO log_auditoria
                (entidad, entidad_id, operacion, usuario_id, fecha, ip_origen, user_agent, modulo_origen, transaccion_id, datos_antes, datos_despues)
                VALUES
                (@entidad, @entidad_id, @operacion, @usuario_id, @fecha, @ip_origen, @user_agent, @modulo_origen, @transaccion_id, @datos_antes::jsonb, @datos_despues::jsonb);";

            await context.Database.ExecuteSqlRawAsync(sql, new[]
            {
                new NpgsqlParameter("entidad", entry.Entity.GetType().Name),
                new NpgsqlParameter("entidad_id", entidadId),
                new NpgsqlParameter("operacion", entry.State.ToString()),
                new NpgsqlParameter("usuario_id", userId ?? (object)DBNull.Value),
                new NpgsqlParameter("fecha", fecha),
                new NpgsqlParameter("ip_origen", ip ?? ""),
                new NpgsqlParameter("user_agent", userAgent ?? ""),
                new NpgsqlParameter("modulo_origen", modulo ?? ""),
                new NpgsqlParameter("transaccion_id", transaccionId),
                new NpgsqlParameter("datos_antes", datosAntes ?? "{}"),
                new NpgsqlParameter("datos_despues", datosDespues ?? "{}")
            }, cancellationToken);
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAuditAsync(eventData.Context!).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await ApplyAuditAsync(eventData.Context!, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
