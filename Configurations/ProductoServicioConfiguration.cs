using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Configurations;

public class ProductoServicioConfiguration : IEntityTypeConfiguration<ProductoServicio>
{
    public void Configure(EntityTypeBuilder<ProductoServicio> builder)
    {
        // ===== Tabla + (opcional) check constraints a nivel tabla =====
        builder.ToTable("producto_servicio", "productos", tb =>
        {
            // Valores no negativos
            tb.HasCheckConstraint("CK_ps_tiempo_no_neg",
                "([dias]    IS NULL OR [dias]    >= 0) AND " +
                "([horas]   IS NULL OR [horas]   >= 0) AND " +
                "([minutos] IS NULL OR [minutos] >= 0) AND " +
                "([segundos]IS NULL OR [segundos]>= 0)");

            // (Opcional) Al menos un componente de tiempo informado
            // tb.HasCheckConstraint("CK_ps_tiempo_al_menos_uno",
            //     "COALESCE([dias],0) + COALESCE([horas],0) + COALESCE([minutos],0) + COALESCE([segundos],0) > 0");
        });

        // ===== Clave =====
        builder.HasKey(x => x.ProductoServicioId);
        builder.Property(x => x.ProductoServicioId)
               .HasColumnName("producto_servicio_id");

        // ===== Columnas =====
        builder.Property(x => x.ProductoId)
               .HasColumnName("producto_id")
               .IsRequired();

        builder.Property(x => x.Dias).HasColumnName("dias");
        builder.Property(x => x.Horas).HasColumnName("horas");
        builder.Property(x => x.Minutos).HasColumnName("minutos");
        builder.Property(x => x.Segundos).HasColumnName("segundos");

        // ===== Índices útiles =====
        builder.HasIndex(x => x.ProductoId)
               .HasDatabaseName("IX_ps_producto");

        // (Opcional) Evitar duplicados si quieres una sola fila por producto
        // builder.HasIndex(x => x.ProductoId)
        //        .IsUnique()
        //        .HasDatabaseName("UX_ps_producto_unico");

        // ===== Relaciones =====
        builder.HasOne(x => x.Producto)
               .WithMany() // o .WithMany(p => p.ProductoServicios) si agregas la colección en Producto
               .HasForeignKey(x => x.ProductoId)
               .OnDelete(DeleteBehavior.Cascade); // al borrar el producto, se borran los servicios
    }
}
