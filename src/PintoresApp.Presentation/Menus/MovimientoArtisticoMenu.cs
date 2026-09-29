using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Menus;

public class MovimientoArtisticoMenu
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MovimientoArtisticoMenu(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== MOVIMIENTOS ARTÍSTICOS =====");
            Console.WriteLine("1. Alta");
            Console.WriteLine("2. Baja");
            Console.WriteLine("3. Modificación");
            Console.WriteLine("4. Listado");
            Console.WriteLine("5. Ver detalle y pintores asociados");
            Console.WriteLine("6. Volver");
            Console.Write("\nOpción: ");

            var opcion = Console.ReadLine()?.Trim();
            Console.WriteLine();

            if (opcion == "6" || opcion == "0") return;

            try
            {
                switch (opcion)
                {
                    case "1": await AltaAsync(); break;
                    case "2": await BajaAsync(); break;
                    case "3": await ModificacionAsync(); break;
                    case "4": await ListadoAsync(); break;
                    case "5": await VerDetalleAsync(); break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Dato inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            ConsoleInput.Pausa();
        }
    }

    private async Task AltaAsync()
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

    private async Task BajaAsync()
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

    private async Task ModificacionAsync()
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

    private async Task ListadoAsync()
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

    private async Task VerDetalleAsync()
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

    private static void MostrarMovimiento(MovimientoArtistico m)
    {
        Console.WriteLine($"\nId: {m.Id}");
        Console.WriteLine($"Nombre: {m.Nombre}");
        Console.WriteLine($"País de origen: {m.PaisOrigen}");
        Console.WriteLine($"Descripción: {m.Descripcion}");
    }
}
