using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Queries;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.Pintor;

public class ListadoPintorStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Listado";

    public ListadoPintorStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Listado de pintores ---\n");

        using var scope = _scopeFactory.CreateScope();

        var pintores = (await scope.ServiceProvider
            .GetRequiredService<ObtenerPintoresQuery>()
            .EjecutarAsync()).ToList();

        if (pintores.Count == 0)
        {
            Console.WriteLine("No hay pintores cargados.");
            return;
        }

        Console.WriteLine($"{"Id",-38}{"Apellido",-18}{"Nombre",-18}{"Nacionalidad",-15}{"Nacimiento",-12}{"Movimiento",-20}");
        Console.WriteLine(new string('-', 121));

        foreach (var p in pintores)
            Console.WriteLine($"{p.Id,-38}{p.Apellido,-18}{p.Nombre,-18}{p.Nacionalidad,-15}{p.FechaNacimiento:dd/MM/yyyy}{p.MovimientoArtistico?.Nombre ?? "-",-20}");
    }
}
