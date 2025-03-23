using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Universal;

public partial class DbIntisoftContext : DbContext
{
    public DbIntisoftContext()
    {
    }

    public DbIntisoftContext(DbContextOptions<DbIntisoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UnTipoDato> UnTipoDatos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost;Database=db_intisoft;username=postgres;password=mx");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnTipoDato>(entity =>
        {
            entity.HasKey(e => e.TipoDatoId).HasName("un_tipo_dato_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
