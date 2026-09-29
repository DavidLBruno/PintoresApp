using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class EliminarMovimientoArtisticoCommand
{
    private readonly IMovimientoArtisticoRepository _repository;

    public EliminarMovimientoArtisticoCommand(IMovimientoArtisticoRepository repository)
    {
        _repository = repository;
    }

    public async Task EjecutarAsync(Guid id)
    {
        var movimiento = await _repository.ObtenerPorIdAsync(id);

        if (movimiento is null)
        {
            throw new Exception("No existe un movimiento artístico con ese ID.");
        }

        await _repository.EliminarAsync(movimiento);
    }
}
