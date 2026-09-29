using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.MovimientoArtistico;

public class BajaMovimientoArtisticoStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Baja";

    public BajaMovimientoArtisticoStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Baja de movimiento artístico ---");
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

        MostrarMovimiento(movimiento);

        if (movimiento.Pintores.Count > 0)
        {
            Console.WriteLine($"\nAtención: Este movimiento tiene {movimiento.Pintores.Count} pintor(es) asociado(s). Al eliminarlo, quedarán sin movimiento asignado.");
        }

        if (!ConsoleInput.Confirmar("\n¿Confirmás la baja?"))
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        await scope.ServiceProvider
            .GetRequiredService<EliminarMovimientoArtisticoCommand>()
            .EjecutarAsync(id);

        Console.WriteLine("Movimiento artístico eliminado.");
    }

    private static void MostrarMovimiento(global::PintoresApp.Domain.Entities.MovimientoArtistico m)
    {
        Console.WriteLine($"\nId: {m.Id}");
        Console.WriteLine($"Nombre: {m.Nombre}");
        Console.WriteLine($"País de origen: {m.PaisOrigen}");
        Console.WriteLine($"Descripción: {m.Descripcion}");
    }
}
