using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Commands;

public class CrearPintorCommand
{
    private readonly IPintorRepository _repository;

    public CrearPintorCommand(IPintorRepository repository)
    {
        _repository = repository;
    }

    public async Task EjecutarAsync(
        string nombre,
        string apellido,
        string nacionalidad,
        DateTime fechaNacimiento,
        Guid? movimientoArtisticoId = null)
    {
        var pintor = new Pintor(
            nombre,
            apellido,
            nacionalidad,
            fechaNacimiento,
            movimientoArtisticoId);

        await _repository.AgregarAsync(pintor);
    }


}