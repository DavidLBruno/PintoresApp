using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.MovimientosArtisticos;

public class AltaMovimientoArtisticoStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Alta";

    public AltaMovimientoArtisticoStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Alta de movimiento artístico ---");

        var nombre = ConsoleInput.LeerTexto("Nombre (ej. Impresionismo)");
        var descripcion = ConsoleInput.LeerTexto("Descripción");
        var paisOrigen = ConsoleInput.LeerTexto("País de origen");

        using var scope = _scopeFactory.CreateScope();
        var crear = scope.ServiceProvider.GetRequiredService<CrearMovimientoArtisticoCommand>();

        await crear.EjecutarAsync(nombre, descripcion, paisOrigen);
        Console.WriteLine("\nMovimiento artístico creado exitosamente.");
    }
}
