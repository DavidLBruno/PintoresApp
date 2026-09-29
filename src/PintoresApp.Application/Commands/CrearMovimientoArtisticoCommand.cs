using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class CrearMovimientoArtisticoCommand
{
    private readonly IMovimientoArtisticoRepository _repository;

    public CrearMovimientoArtisticoCommand(IMovimientoArtisticoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> EjecutarAsync(string nombre, string descripcion, string paisOrigen)
    {
        var movimiento = new MovimientoArtistico(nombre, descripcion, paisOrigen);
        await _repository.AgregarAsync(movimiento);
        return movimiento.Id;
    }
}
