using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Queries;

public class ObtenerMovimientosArtisticosQuery
{
    private readonly IMovimientoArtisticoRepository _repository;

    public ObtenerMovimientosArtisticosQuery(IMovimientoArtisticoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MovimientoArtistico>> EjecutarAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }
}
