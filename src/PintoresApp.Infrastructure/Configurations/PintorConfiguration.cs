using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PintoresApp.Domain.Entities;

namespace PintoresApp.Infrastructure.Persistence.Configurations;

public class PintorConfiguration : IEntityTypeConfiguration<Pintor>
{
    public void Configure(EntityTypeBuilder<Pintor> builder)
    {
        builder.ToTable("Pintores");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(Pintor.LongitudMaximaTexto);

        builder.Property(p => p.Apellido)
            .IsRequired()
            .HasMaxLength(Pintor.LongitudMaximaTexto);

        builder.Property(p => p.Nacionalidad)
            .IsRequired()
            .HasMaxLength(Pintor.LongitudMaximaTexto);

        builder.Property(p => p.FechaNacimiento)
            .IsRequired();

        builder.HasOne(p => p.MovimientoArtistico)
            .WithMany(m => m.Pintores)
            .HasForeignKey(p => p.MovimientoArtisticoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}