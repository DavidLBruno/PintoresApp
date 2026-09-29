using Microsoft.EntityFrameworkCore;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;
using PintoresApp.Infrastructure.Persistence;

namespace PintoresApp.Infrastructure.Repositories;

public class MovimientoArtisticoRepository : IMovimientoArtisticoRepository
{
    private readonly AppDbContext _context;

    public MovimientoArtisticoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovimientoArtistico>> ObtenerTodosAsync()
        => await _context.MovimientosArtisticos
            .AsNoTracking()
            .Include(m => m.Pintores)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

    public async Task<MovimientoArtistico?> ObtenerPorIdAsync(Guid id)
        => await _context.MovimientosArtisticos
            .Include(m => m.Pintores)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task AgregarAsync(MovimientoArtistico movimiento)
    {
        await _context.MovimientosArtisticos.AddAsync(movimiento);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(MovimientoArtistico movimiento)
    {
        _context.MovimientosArtisticos.Update(movimiento);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(MovimientoArtistico movimiento)
    {
        _context.MovimientosArtisticos.Remove(movimiento);
        await _context.SaveChangesAsync();
    }
}
