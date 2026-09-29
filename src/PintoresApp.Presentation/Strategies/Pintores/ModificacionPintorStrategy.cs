using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.Pintores;

public class ModificacionPintorStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Modificación";

    public ModificacionPintorStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Modificación de pintor ---");
        var id = ConsoleInput.LeerGuid("Id");

        using var scope = _scopeFactory.CreateScope();

        var pintor = await scope.ServiceProvider
            .GetRequiredService<ObtenerPintorPorIdQuery>()
            .EjecutarAsync(id);

        if (pintor is null)
        {
            Console.WriteLine("No existe un pintor con ese Id.");
            return;
        }

        Console.WriteLine("Enter para dejar el valor actual.\n");

        var nombre = ConsoleInput.LeerTexto("Nombre", pintor.Nombre);
        var apellido = ConsoleInput.LeerTexto("Apellido", pintor.Apellido);
        var nacionalidad = ConsoleInput.LeerTexto("Nacionalidad", pintor.Nacionalidad);
        var fecha = ConsoleInput.LeerFecha("Fecha de nacimiento", pintor.FechaNacimiento);
        var movimientoId = await MovimientoSelector.SeleccionarAsync(_scopeFactory, pintor.MovimientoArtisticoId);

        await scope.ServiceProvider
            .GetRequiredService<ActualizarPintorCommand>()
            .EjecutarAsync(id, nombre, apellido, nacionalidad, fecha, movimientoId);

        Console.WriteLine("\nPintor actualizado.");
    }
}
