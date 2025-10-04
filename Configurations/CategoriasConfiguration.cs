using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class CategoriasConfiguration : IEntityTypeConfiguration<Categorias>
{
    public void Configure(EntityTypeBuilder<Categorias> builder)
    {
        // Tabla y clave
        builder.ToTable("categoria", "productos");
        builder.HasKey(c => c.CategoriaId);
        builder.Property(c => c.CategoriaId).HasColumnName("categoria_id");

        // Columnas
        builder.Property(c => c.CategoriaMainId).HasColumnName("categoria_main_id");
        builder.Property(c => c.IsEnd).HasColumnName("is_end");

        builder.Property(c => c.Categoria)
               .HasColumnName("categoria")
               .HasMaxLength(32)
               .IsRequired();

        builder.Property(c => c.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(128);

        builder.Property(c => c.Activo).HasColumnName("activo");

        builder.Property(c => c.IsEditadoFichaTecnica)
               .HasColumnName("is_editado_ficha_tecnica");

        builder.Property(c => c.CategoriaIdFichaTecnica)
               .HasColumnName("categoria_id_ficha_tecnica");
        //orden Integer
        builder.Property(c => c.Orden).HasColumnName("orden");

        //aliass varchar(5)
        builder.Property(c => c.Aliass)
               .HasColumnName("aliass")
               .HasMaxLength(5);

        // Índices
        builder.HasIndex(c => c.Categoria)
               .HasDatabaseName("IX_categoria_nombre");

        builder.HasIndex(c => new { c.Activo, c.IsEnd })
               .HasDatabaseName("IX_categoria_activo_isend");

        // Relación 1:N con Producto (expresa también aquí por simetría/claridad)
        builder.HasMany(c => c.Productos)
               .WithOne(p => p.Categoria)
               .HasForeignKey(p => p.CategoriaId)
               .OnDelete(DeleteBehavior.SetNull);

        // Si más adelante necesitas configurar CategoriaFichaTecnicas de forma explícita,
        // crea su Configuration para definir FK y delete behavior exacto.
    }
}
