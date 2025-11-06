using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

public partial class DbCentroEstudiosContext : DbContext
{
    public DbCentroEstudiosContext()
    {
    }

    public DbCentroEstudiosContext(DbContextOptions<DbCentroEstudiosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoEvaluacionProgramadum> EstadoEvaluacionProgramada { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;DataBase=db_centro_estudios;Port=5432;User Id=postgres;Password=mx");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("academia", "asistencia_estado_enum", new[] { "P", "A", "T" })
            .HasPostgresEnum("academia", "asistencia_fuente_enum", new[] { "QR", "LISTA" })
            .HasPostgresEnum("academia", "ciclo_estado_enum", new[] { "borrador", "publicado", "inscribible", "en_curso", "cerrado" })
            .HasPostgresEnum("academia", "combo_estado_enum", new[] { "activo", "inactivo" })
            .HasPostgresEnum("academia", "evaluacion_estado_enum", new[] { "pendiente", "en_proceso", "corregida" })
            .HasPostgresEnum("academia", "matricula_estado_enum", new[] { "preinscrito", "pendiente_pago", "activo", "suspendido", "retirado", "finalizado" })
            .HasPostgresEnum("academia", "orden_estado_enum", new[] { "pendiente", "pagado", "anulado", "devuelto" })
            .HasPostgresEnum("academia", "pago_estado_enum", new[] { "pendiente", "confirmado", "anulado" })
            .HasPostgresEnum("academia", "pago_metodo_enum", new[] { "efectivo", "pos", "transferencia", "yape", "plin", "pasarela" })
            .HasPostgresEnum("academia", "rol_enum", new[] { "admin", "academico", "caja", "docente", "asesor", "alumno", "apoderado", "auditor" })
            .HasPostgresEnum("academia", "seccion_estado_enum", new[] { "activa", "inactiva" })
            .HasPostgresEnum("academia", "sede_estado_enum", new[] { "activa", "inactiva" })
            .HasPostgresEnum("academia", "sexo_enum", new[] { "m", "f", "x" })
            .HasPostgresEnum("academia", "simulacro_estado_enum", new[] { "programado", "habilitado", "cerrado", "publicado" })
            .HasPostgresEnum("academia", "simulacro_target_enum", new[] { "nivel", "carrera", "ambos" })
            .HasPostgresEnum("academia", "sunat_estado_enum", new[] { "no_enviado", "pendiente", "aceptado", "observado", "rechazado" });

        modelBuilder.Entity<EstadoEvaluacionProgramadum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_estado_ep");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });
        modelBuilder.HasSequence("alumno_apoderado_id_seq", "academia");
        modelBuilder.HasSequence("alumno_id_seq", "academia");
        modelBuilder.HasSequence("alumno_usuario_id_seq", "academia");
        modelBuilder.HasSequence("apertura_ciclo_id_seq", "academia");
        modelBuilder.HasSequence("apertura_seccion_id_seq", "academia");
        modelBuilder.HasSequence("apoderado_id_seq", "academia");
        modelBuilder.HasSequence("archivo_id_seq", "academia");
        modelBuilder.HasSequence("asistencia_id_seq", "academia");
        modelBuilder.HasSequence("audit_log_id_seq", "academia");
        modelBuilder.HasSequence("beca_alumno_id_seq", "academia");
        modelBuilder.HasSequence("beca_id_seq", "academia");
        modelBuilder.HasSequence("carrera_id_seq", "academia");
        modelBuilder.HasSequence("ciclo_id_seq", "academia");
        modelBuilder.HasSequence("colegio_id_seq", "academia");
        modelBuilder.HasSequence("combo_id_seq", "academia");
        modelBuilder.HasSequence("combo_item_id_seq", "academia");
        modelBuilder.HasSequence("concepto_id_seq", "academia");
        modelBuilder.HasSequence("concepto_tipo_id_seq", "academia");
        modelBuilder.HasSequence("curso_docente_id_seq", "academia");
        modelBuilder.HasSequence("curso_id_seq", "academia");
        modelBuilder.HasSequence("devolucion_id_seq", "academia");
        modelBuilder.HasSequence("docente_id_seq", "academia");
        modelBuilder.HasSequence("docente_usuario_id_seq", "academia");
        modelBuilder.HasSequence("especialidad_id_seq", "academia");
        modelBuilder.HasSequence("estado_evaluacion_programada_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_clave_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_detalle_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_nota_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_programada_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_programada_seccion_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_respuesta_id_seq", "academia");
        modelBuilder.HasSequence("evaluacion_tipo_pregunta_id_seq", "academia");
        modelBuilder.HasSequence("matricula_id_seq", "academia");
        modelBuilder.HasSequence("matricula_item_id_seq", "academia");
        modelBuilder.HasSequence("nivel_id_seq", "academia");
        modelBuilder.HasSequence("nota_id_seq", "academia");
        modelBuilder.HasSequence("omr_captura_id_seq", "academia");
        modelBuilder.HasSequence("orden_id_seq", "academia");
        modelBuilder.HasSequence("orden_item_id_seq", "academia");
        modelBuilder.HasSequence("pago_id_seq", "academia");
        modelBuilder.HasSequence("parentesco_id_seq", "academia");
        modelBuilder.HasSequence("seccion_ciclo_id_seq", "academia");
        modelBuilder.HasSequence("seccion_id_seq", "academia");
        modelBuilder.HasSequence("sede_id_seq", "academia");
        modelBuilder.HasSequence("sg_rol_rol_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("sg_usuario_usuario_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("simulacro_detalle_id_seq", "academia");
        modelBuilder.HasSequence("simulacro_id_seq", "academia");
        modelBuilder.HasSequence("simulado_clave_id_seq", "academia");
        modelBuilder.HasSequence("simulado_forma_id_seq", "academia");
        modelBuilder.HasSequence("simulado_id_seq", "academia");
        modelBuilder.HasSequence("simulado_pregunta_id_seq", "academia");
        modelBuilder.HasSequence("tipo_evaluacion_id_seq", "academia");
        modelBuilder.HasSequence("tipo_simulacro_id_seq", "academia");
        modelBuilder.HasSequence("ubigeo_id_seq", "academia");
        modelBuilder.HasSequence("universidad_id_seq", "academia");
        modelBuilder.HasSequence("usuario_id_seq", "academia");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
