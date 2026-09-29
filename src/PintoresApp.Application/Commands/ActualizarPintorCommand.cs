using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class ActualizarPintorCommand
{
    private readonly IPintorRepository _repository;

    public ActualizarPintorCommand(IPintorRepository repository)
    {
        _repository = repository;
    }

    public async Task EjecutarAsync(
        Guid id,
        string nombre,
        string apellido,
        string nacionalidad,
        DateTime fechaNacimiento,
        Guid? movimientoArtisticoId = null)
    {
        var pintor = await _repository.ObtenerPorIdAsync(id);

        if (pintor == null)
        {
            throw new Exception("No existe un pintor con ese ID.");
        }

        pintor.Actualizar(
            nombre,
            apellido,
            nacionalidad,
            fechaNacimiento,
            movimientoArtisticoId);

        await _repository.ActualizarAsync(pintor);
    }


}