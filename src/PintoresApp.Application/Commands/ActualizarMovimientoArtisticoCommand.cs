using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class ActualizarMovimientoArtisticoCommand
{
    private readonly IMovimientoArtisticoRepository _repository;

    public ActualizarMovimientoArtisticoCommand(IMovimientoArtisticoRepository repository)
    {
        _repository = repository;
    }

    public async Task EjecutarAsync(Guid id, string nombre, string descripcion, string paisOrigen)
    {
        var movimiento = await _repository.ObtenerPorIdAsync(id);

        if (movimiento is null)
        {
            throw new Exception("No existe un movimiento artístico con ese ID.");
        }

        movimiento.Actualizar(nombre, descripcion, paisOrigen);
        await _repository.ActualizarAsync(movimiento);
    }
}
