using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Menus;

public class PintorMenu
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PintorMenu(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== PINTORES =====");
            Console.WriteLine("1. Alta");
            Console.WriteLine("2. Baja");
            Console.WriteLine("3. Modificación");
            Console.WriteLine("4. Listado");
            Console.WriteLine("5. Salir");
            Console.Write("\nOpción: ");

            var opcion = Console.ReadLine()?.Trim();
            Console.WriteLine();

            if (opcion == "5" || opcion == "0") return;

            try
            {
                switch (opcion)
                {
                    case "1": await AltaAsync(); break;
                    case "2": await BajaAsync(); break;
                    case "3": await ModificacionAsync(); break;
                    case "4": await ListadoAsync(); break;
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
        Console.WriteLine("--- Alta de pintor ---");

        var nombre = ConsoleInput.LeerTexto("Nombre");
        var apellido = ConsoleInput.LeerTexto("Apellido");
        var nacionalidad = ConsoleInput.LeerTexto("Nacionalidad");
        var fecha = ConsoleInput.LeerFecha("Fecha de nacimiento");
        var movimientoId = await SeleccionarMovimientoAsync();

        using var scope = _scopeFactory.CreateScope();
        var crear = scope.ServiceProvider.GetRequiredService<CrearPintorCommand>();

        await crear.EjecutarAsync(nombre, apellido, nacionalidad, fecha, movimientoId);
        Console.WriteLine("\nPintor creado exitosamente.");
    }

    private async Task BajaAsync()
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

    private async Task ModificacionAsync()
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
        var movimientoId = await SeleccionarMovimientoAsync(pintor.MovimientoArtisticoId);

        await scope.ServiceProvider
            .GetRequiredService<ActualizarPintorCommand>()
            .EjecutarAsync(id, nombre, apellido, nacionalidad, fecha, movimientoId);

        Console.WriteLine("\nPintor actualizado.");
    }

    private async Task ListadoAsync()
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

    private static void MostrarPintor(Pintor p)
    {
        Console.WriteLine($"\nId: {p.Id}");
        Console.WriteLine($"Nombre: {p.Nombre} {p.Apellido}");
        Console.WriteLine($"Nacionalidad: {p.Nacionalidad}");
        Console.WriteLine($"Nacimiento: {p.FechaNacimiento:dd/MM/yyyy}");
        Console.WriteLine($"Movimiento artístico: {p.MovimientoArtistico?.Nombre ?? "Sin asignar"}");
    }

    private async Task<Guid?> SeleccionarMovimientoAsync(Guid? movimientoActualId = null)
    {
        using var scope = _scopeFactory.CreateScope();
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