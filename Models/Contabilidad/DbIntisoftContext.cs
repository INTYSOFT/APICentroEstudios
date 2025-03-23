using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

public partial class DbIntisoftContext : DbContext
{
    public DbIntisoftContext()
    {
    }

    public DbIntisoftContext(DbContextOptions<DbIntisoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CtComprobante> CtComprobantes { get; set; }

    public virtual DbSet<CtDeclaracionesTributaria> CtDeclaracionesTributarias { get; set; }

    public virtual DbSet<CtDocumentosTributario> CtDocumentosTributarios { get; set; }

    public virtual DbSet<CtElementoCuentaContable> CtElementoCuentaContables { get; set; }

    public virtual DbSet<CtEstadoDocumento> CtEstadoDocumentos { get; set; }

    public virtual DbSet<CtLibroDiario> CtLibroDiarios { get; set; }

    public virtual DbSet<CtLibroInventariosBalance> CtLibroInventariosBalances { get; set; }

    public virtual DbSet<CtLibroMayor> CtLibroMayors { get; set; }

    public virtual DbSet<CtNumeracionComprobante> CtNumeracionComprobantes { get; set; }

    public virtual DbSet<CtPercepcionesRetencione> CtPercepcionesRetenciones { get; set; }

    public virtual DbSet<CtPlanContable> CtPlanContables { get; set; }

    public virtual DbSet<CtTipoDeclaracion> CtTipoDeclaracions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost;Database=db_intisoft;username=postgres;password=mx");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CtComprobante>(entity =>
        {
            entity.HasKey(e => e.ComprobanteId).HasName("lg_comprobante_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<CtDeclaracionesTributaria>(entity =>
        {
            entity.HasKey(e => e.DeclaracionId).HasName("ct_declaraciones_tributarias_pkey");

            entity.HasOne(d => d.TipoDeclaracion).WithMany(p => p.CtDeclaracionesTributaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ct_declaraciones_tributarias_tipo_declaracion_id_fkey");
        });

        modelBuilder.Entity<CtDocumentosTributario>(entity =>
        {
            entity.HasKey(e => e.DocumentoId).HasName("ct_documentos_tributarios_pkey");

            entity.HasOne(d => d.Comprobante).WithMany(p => p.CtDocumentosTributarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comprobante_id_doc_tributarios");

            entity.HasOne(d => d.EstadoDocumento).WithMany(p => p.CtDocumentosTributarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ct_documentos_tributarios_estado_documento_id_fkey");
        });

        modelBuilder.Entity<CtElementoCuentaContable>(entity =>
        {
            entity.HasKey(e => e.CodigoElementoContableId).HasName("ct_elemento_cuenta_contable_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<CtEstadoDocumento>(entity =>
        {
            entity.HasKey(e => e.EstadoDocumentoId).HasName("ct_estado_documento_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<CtLibroDiario>(entity =>
        {
            entity.HasKey(e => e.AsientoId).HasName("ct_libro_diario_pkey");
        });

        modelBuilder.Entity<CtLibroInventariosBalance>(entity =>
        {
            entity.HasKey(e => e.InventarioId).HasName("ct_libro_inventarios_balances_pkey");
        });

        modelBuilder.Entity<CtLibroMayor>(entity =>
        {
            entity.HasKey(e => e.MayorId).HasName("ct_libro_mayor_pkey");
        });

        modelBuilder.Entity<CtNumeracionComprobante>(entity =>
        {
            entity.HasKey(e => e.NumeracionComprobanteId).HasName("ct_numeracion_comprobante_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.Comprobante).WithMany(p => p.CtNumeracionComprobantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comprobante_id");
        });

        modelBuilder.Entity<CtPercepcionesRetencione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ct_percepciones_retenciones_pkey");
        });

        modelBuilder.Entity<CtPlanContable>(entity =>
        {
            entity.HasKey(e => e.PlanContableId).HasName("ct_plan_contable_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.CodigoElementoContable).WithMany(p => p.CtPlanContables)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ct_plan_contable_codigo_elemento_contable_id_fkey");
        });

        modelBuilder.Entity<CtTipoDeclaracion>(entity =>
        {
            entity.HasKey(e => e.TipoDeclaracionId).HasName("ct_tipo_declaracion_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
