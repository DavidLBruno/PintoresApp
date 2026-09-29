using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Queries;

namespace PintoresApp.Presentation.Helpers;

/// <summary>
/// Helper compartido por las estrategias de Pintor para seleccionar un MovimientoArtístico.
/// </summary>
public static class MovimientoSelector
{
    public static async Task<Guid?> SeleccionarAsync(
        IServiceScopeFactory scopeFactory,
        Guid? movimientoActualId = null)
    {
        using var scope = scopeFactory.CreateScope();
        var movimientos = (await scope.ServiceProvider
            .GetRequiredService<ObtenerMovimientosArtisticosQuery>()
            .EjecutarAsync()).ToList();

        if (movimientos.Count == 0)
        {
            Console.WriteLine("No hay movimientos artísticos cargados aún (se puede asignar luego).");
            return null;
        }

        Console.WriteLine("\n--- Asignación de Movimiento Artístico ---");
        Console.WriteLine("0. Ninguno / Sin asignar");
        for (int i = 0; i < movimientos.Count; i++)
        {
            var m = movimientos[i];
            var marca = m.Id == movimientoActualId ? " [Actual]" : "";
            Console.WriteLine($"{i + 1}. {m.Nombre} ({m.PaisOrigen}){marca}");
        }

        string valorDefault = movimientoActualId.HasValue
            ? (movimientos.FindIndex(x => x.Id == movimientoActualId) + 1).ToString()
            : "0";

        while (true)
        {
            var entrada = ConsoleInput.LeerTexto("Seleccioná número de movimiento", valorDefault);
            if (int.TryParse(entrada, out int seleccionado))
            {
                if (seleccionado == 0) return null;
                if (seleccionado > 0 && seleccionado <= movimientos.Count)
                    return movimientos[seleccionado - 1].Id;
            }

            Console.WriteLine("Opción no válida.");
        }
    }
}
