using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Presentation.Strategies;
using PintoresApp.Presentation.Strategies.MovimientosArtisticos;

namespace PintoresApp.Presentation.Menus;

/// <summary>
/// Cliente del patrón Strategy para el menú de Movimientos Artísticos.
/// Configura las estrategias disponibles y le pasa la lista al MenuContext,
/// que se encarga de presentar el menú y despachar la ejecución.
/// </summary>
public class MovimientoArtisticoMenu
{
    private readonly MenuContext _context;

    public MovimientoArtisticoMenu(IServiceScopeFactory scopeFactory)
    {
        _context = new MenuContext("MOVIMIENTOS ARTÍSTICOS",
        [
            new AltaMovimientoArtisticoStrategy(scopeFactory),
            new BajaMovimientoArtisticoStrategy(scopeFactory),
            new ModificacionMovimientoArtisticoStrategy(scopeFactory),
            new ListadoMovimientoArtisticoStrategy(scopeFactory),
            new DetalleMovimientoArtisticoStrategy(scopeFactory),
        ]);
    }

    public Task EjecutarAsync() => _context.EjecutarAsync();
}
