using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.MovimientoArtistico;

public class ModificacionMovimientoArtisticoStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Modificación";

    public ModificacionMovimientoArtisticoStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Modificación de movimiento artístico ---");
        var id = ConsoleInput.LeerGuid("Id");

        using var scope = _scopeFactory.CreateScope();
        var movimiento = await scope.ServiceProvider
            .GetRequiredService<ObtenerMovimientoArtisticoPorIdQuery>()
            .EjecutarAsync(id);

        if (movimiento is null)
        {
            Console.WriteLine("No existe un movimiento artístico con ese Id.");
            return;
        }

        Console.WriteLine("Enter para dejar el valor actual.\n");

        var nombre = ConsoleInput.LeerTexto("Nombre", movimiento.Nombre);
        var descripcion = ConsoleInput.LeerTexto("Descripción", movimiento.Descripcion);
        var paisOrigen = ConsoleInput.LeerTexto("País de origen", movimiento.PaisOrigen);

        await scope.ServiceProvider
            .GetRequiredService<ActualizarMovimientoArtisticoCommand>()
            .EjecutarAsync(id, nombre, descripcion, paisOrigen);

        Console.WriteLine("\nMovimiento artístico actualizado.");
    }
}
