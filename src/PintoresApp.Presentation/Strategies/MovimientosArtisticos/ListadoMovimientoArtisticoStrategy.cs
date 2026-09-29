using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Queries;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.MovimientosArtisticos;

public class ListadoMovimientoArtisticoStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Listado";

    public ListadoMovimientoArtisticoStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Listado de movimientos artísticos ---\n");

        using var scope = _scopeFactory.CreateScope();
        var movimientos = (await scope.ServiceProvider
            .GetRequiredService<ObtenerMovimientosArtisticosQuery>()
            .EjecutarAsync()).ToList();

        if (movimientos.Count == 0)
        {
            Console.WriteLine("No hay movimientos artísticos cargados.");
            return;
        }

        Console.WriteLine($"{"Id",-38}{"Nombre",-25}{"País Origen",-20}{"Pintores",-10}");
        Console.WriteLine(new string('-', 93));

        foreach (var m in movimientos)
        {
            Console.WriteLine($"{m.Id,-38}{m.Nombre,-25}{m.PaisOrigen,-20}{m.Pintores.Count,-10}");
        }
    }
}
