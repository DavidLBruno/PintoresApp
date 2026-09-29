using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Queries;

public class ObtenerMovimientoArtisticoPorIdQuery
{
    private readonly IMovimientoArtisticoRepository _repository;

    public ObtenerMovimientoArtisticoPorIdQuery(IMovimientoArtisticoRepository repository)
    {
        _repository = repository;
    }

    public async Task<MovimientoArtistico?> EjecutarAsync(Guid id)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }
}
