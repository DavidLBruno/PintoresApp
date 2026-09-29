using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application;
using PintoresApp.Infrastructure;
using PintoresApp.Presentation.Helpers;
using PintoresApp.Presentation.Menus;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var rutaDb = Path.Combine(AppContext.BaseDirectory, "pintores.db");

var services = new ServiceCollection();
services.AddApplication();
services.AddInfrastructure($"Data Source={rutaDb}");
services.AddSingleton<PintorMenu>();
services.AddSingleton<MovimientoArtisticoMenu>();

using var provider = services.BuildServiceProvider();

provider.InicializarBaseDeDatos();

var pintorMenu = provider.GetRequiredService<PintorMenu>();
var movimientoMenu = provider.GetRequiredService<MovimientoArtisticoMenu>();

while (true)
{
    Console.Clear();
    Console.WriteLine("======================================");
    Console.WriteLine("      SISTEMA DE GESTIÓN DE ARTE      ");
    Console.WriteLine("======================================");
    Console.WriteLine("1. Gestión de Pintores");
    Console.WriteLine("2. Gestión de Movimientos Artísticos");
    Console.WriteLine("3. Salir");
    Console.Write("\nOpción: ");

    var opcion = Console.ReadLine()?.Trim();

    if (opcion == "3" || opcion == "0")
    {
        Console.WriteLine("\n¡Hasta luego!");
        break;
    }

    switch (opcion)
    {
        case "1":
            await pintorMenu.EjecutarAsync();
            break;
        case "2":
            await movimientoMenu.EjecutarAsync();
            break;
        default:
            Console.WriteLine("Opción no válida.");
            ConsoleInput.Pausa();
            break;
    }
}