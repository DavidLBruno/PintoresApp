using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.MovimientoArtistico;

public class DetalleMovimientoArtisticoStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Ver detalle y pintores asociados";

    public DetalleMovimientoArtisticoStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Detalle de movimiento artístico ---");
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

        Console.WriteLine("\n--- Pintores pertenecientes a este movimiento ---");
        if (movimiento.Pintores.Count == 0)
        {
            Console.WriteLine("No hay pintores asignados a este movimiento aún.");
        }
        else
        {
            foreach (var pintor in movimiento.Pintores)
            {
                Console.WriteLine($"- {pintor.Apellido}, {pintor.Nombre} ({pintor.Nacionalidad})");
            }
        }
    }

    private static void MostrarMovimiento(global::PintoresApp.Domain.Entities.MovimientoArtistico m)
    {
        Console.WriteLine($"\nId: {m.Id}");
        Console.WriteLine($"Nombre: {m.Nombre}");
        Console.WriteLine($"País de origen: {m.PaisOrigen}");
        Console.WriteLine($"Descripción: {m.Descripcion}");
    }
}
