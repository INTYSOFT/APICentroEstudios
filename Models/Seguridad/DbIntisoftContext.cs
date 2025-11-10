using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Seguridad;

public partial class DbIntisoftContext : DbContext
{
    public DbIntisoftContext()
    {
    }

    public DbIntisoftContext(DbContextOptions<DbIntisoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SgUsuario> SgUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;DataBase=db_intisoft;Port=5432;User Id=postgres;Password=mx");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SgUsuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("sg_usuario_pkey");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });
        modelBuilder.HasSequence("lg_categoria_ficha_tecnica_id_seq");
        modelBuilder.HasSequence("lg_marca_lg_marca_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
