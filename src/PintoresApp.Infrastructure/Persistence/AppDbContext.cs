using Microsoft.EntityFrameworkCore;
using PintoresApp.Domain.Entities;

namespace PintoresApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pintor> Pintores => Set<Pintor>();
    public DbSet<MovimientoArtistico> MovimientosArtisticos => Set<MovimientoArtistico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}