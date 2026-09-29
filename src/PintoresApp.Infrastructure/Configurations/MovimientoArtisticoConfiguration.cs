using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PintoresApp.Domain.Entities;

namespace PintoresApp.Infrastructure.Persistence.Configurations;

public class MovimientoArtisticoConfiguration : IEntityTypeConfiguration<MovimientoArtistico>
{
    public void Configure(EntityTypeBuilder<MovimientoArtistico> builder)
    {
        builder.ToTable("MovimientosArtisticos");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Nombre)
            .IsRequired()
            .HasMaxLength(MovimientoArtistico.LongitudMaximaTexto);

        builder.Property(m => m.Descripcion)
            .IsRequired()
            .HasMaxLength(MovimientoArtistico.LongitudMaximaDescripcion);

        builder.Property(m => m.PaisOrigen)
            .IsRequired()
            .HasMaxLength(MovimientoArtistico.LongitudMaximaTexto);

        builder.HasMany(m => m.Pintores)
            .WithOne(p => p.MovimientoArtistico)
            .HasForeignKey(p => p.MovimientoArtisticoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
