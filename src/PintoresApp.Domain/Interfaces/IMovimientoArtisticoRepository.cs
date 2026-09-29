using PintoresApp.Domain.Entities;

namespace PintoresApp.Domain.Interfaces;

public interface IMovimientoArtisticoRepository
{
    Task<IEnumerable<MovimientoArtistico>> ObtenerTodosAsync();
    Task<MovimientoArtistico?> ObtenerPorIdAsync(Guid id);
    Task AgregarAsync(MovimientoArtistico movimiento);
    Task ActualizarAsync(MovimientoArtistico movimiento);
    Task EliminarAsync(MovimientoArtistico movimiento);
}
