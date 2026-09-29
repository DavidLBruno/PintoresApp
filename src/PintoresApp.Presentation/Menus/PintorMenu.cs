using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Presentation.Strategies;
using PintoresApp.Presentation.Strategies.Pintor;

namespace PintoresApp.Presentation.Menus;

/// <summary>
/// Contexto del patrón Strategy para el menú de Pintores.
/// Registra las estrategias disponibles y delega en MenuContext la presentación
/// y despacho del menú.
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