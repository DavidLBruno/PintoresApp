using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class EliminarPintorCommand
{
    private readonly IPintorRepository _repository;

    public EliminarPintorCommand(IPintorRepository repository)
    {
        _repository = repository;
    }

    public async Task EjecutarAsync(Guid id)
    {
        var pintor = await _repository.ObtenerPorIdAsync(id);

        if (pintor == null)
        {
            throw new Exception("No existe un pintor con ese ID.");
        }

        await _repository.EliminarAsync(pintor);
    }
}