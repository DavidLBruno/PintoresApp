using Microsoft.EntityFrameworkCore;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;
using PintoresApp.Infrastructure.Persistence;

namespace PintoresApp.Infrastructure.Repositories;

public class PintorRepository : IPintorRepository
{
    private readonly AppDbContext _context;

    public PintorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pintor>> ObtenerTodosAsync()
        => await _context.Pintores
            .AsNoTracking()
            .Include(p => p.MovimientoArtistico)
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

    public async Task<Pintor?> ObtenerPorIdAsync(Guid id)
        => await _context.Pintores
            .Include(p => p.MovimientoArtistico)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AgregarAsync(Pintor pintor)
    {
        await _context.Pintores.AddAsync(pintor);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Pintor pintor)
    {
        _context.Pintores.Update(pintor);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Pintor pintor)
    {
        _context.Pintores.Remove(pintor);
        await _context.SaveChangesAsync();
    }
}