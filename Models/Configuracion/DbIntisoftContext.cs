using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Configuracion;

public partial class DbIntisoftContext : DbContext
{
    public DbIntisoftContext()
    {
    }

    public DbIntisoftContext(DbContextOptions<DbIntisoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CfProducto> CfProductos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost;Database=db_intisoft;username=postgres;password=mx");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CfProducto>(entity =>
        {
            entity.HasKey(e => e.ProductoConfigId).HasName("cf_producto_pkey");

            entity.Property(e => e.WithCodigoBarras).HasDefaultValue(false);
            entity.Property(e => e.WithCodigoInterno).HasDefaultValue(false);
            entity.Property(e => e.WithCodigoQr).HasDefaultValue(false);
            entity.Property(e => e.WithDimensionesPeso).HasDefaultValue(false);
            entity.Property(e => e.WithGestionIncluyePresentacion).HasDefaultValue(false);
            //entity.Property(e => e.WithVarianteSku).HasDefaultValue(false);
            entity.Property(e => e.WithVariantes).HasDefaultValue(false);
        });
        modelBuilder.HasSequence("lg_categoria_ficha_tecnica_id_seq");
        modelBuilder.HasSequence("lg_marca_lg_marca_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
