using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Presentation.Strategies;
using PintoresApp.Presentation.Strategies.Pintores;

namespace PintoresApp.Presentation.Menus;

/// <summary>
/// Cliente del patrón Strategy para el menú de Pintores.
/// Configura las estrategias disponibles y le pasa la lista al MenuContext,
/// que se encarga de presentar el menú y despachar la ejecución.
/// </summary>
public class PintorMenu
{
    private readonly MenuContext _context;

    public PintorMenu(IServiceScopeFactory scopeFactory)
    {
        _context = new MenuContext("PINTORES",
        [
            new AltaPintorStrategy(scopeFactory),
            new BajaPintorStrategy(scopeFactory),
            new ModificacionPintorStrategy(scopeFactory),
            new ListadoPintorStrategy(scopeFactory),
        ]);
    }

    public Task EjecutarAsync() => _context.EjecutarAsync();
}