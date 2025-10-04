using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ConversionUnidadConfiguration : IEntityTypeConfiguration<ConversionUnidad>
{
    public void Configure(EntityTypeBuilder<ConversionUnidad> builder)
    {
        builder.ToTable("conversion_unidad", "productos", tb =>
        {
            tb.HasCheckConstraint("CK_conv_factor_gt_zero", "[factor_conversion] > 0");
            tb.HasCheckConstraint("CK_conv_origen_neq_destino", "[unidad_origen_id] <> [unidad_destino_id]");
        });

        builder.HasKey(x => x.ConversionId);
        builder.Property(x => x.ConversionId).HasColumnName("conversion_id");
        builder.Property(x => x.UnidadOrigenId).HasColumnName("unidad_origen_id").IsRequired();
        builder.Property(x => x.UnidadDestinoId).HasColumnName("unidad_destino_id").IsRequired();
        builder.Property(x => x.FactorConversion).HasColumnName("factor_conversion").HasPrecision(18, 6).IsRequired();

        builder.HasIndex(x => new { x.UnidadOrigenId, x.UnidadDestinoId })
               .IsUnique()
               .HasDatabaseName("UX_conv_unidad_origen_destino");

        // 🔹 Relación: Origen
        builder.HasOne(c => c.UnidadOrigen)
               .WithMany(u => u.LgConversionUnidadUnidadOrigens)
               .HasForeignKey(c => c.UnidadOrigenId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_conv_unidad_origen");

        // 🔹 Relación: Destino
        builder.HasOne(c => c.UnidadDestino)
               .WithMany(u => u.LgConversionUnidadUnidadDestinos)
               .HasForeignKey(c => c.UnidadDestinoId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_conv_unidad_destino");
    }
}
