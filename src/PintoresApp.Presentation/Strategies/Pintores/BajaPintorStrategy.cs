using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.Pintores;

public class BajaPintorStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Baja";

    public BajaPintorStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Baja de pintor ---");
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

        MostrarPintor(pintor);

        if (!ConsoleInput.Confirmar("\n¿Confirmás la baja?"))
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        await scope.ServiceProvider
            .GetRequiredService<EliminarPintorCommand>()
            .EjecutarAsync(id);

        Console.WriteLine("Pintor eliminado.");
    }

    private static void MostrarPintor(Pintor p)
    {
        Console.WriteLine($"\nId: {p.Id}");
        Console.WriteLine($"Nombre: {p.Nombre} {p.Apellido}");
        Console.WriteLine($"Nacionalidad: {p.Nacionalidad}");
        Console.WriteLine($"Nacimiento: {p.FechaNacimiento:dd/MM/yyyy}");
        Console.WriteLine($"Movimiento artístico: {p.MovimientoArtistico?.Nombre ?? "Sin asignar"}");
    }
}
