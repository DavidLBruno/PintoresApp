using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Presentation.Strategies;
using PintoresApp.Presentation.Strategies.MovimientoArtistico;

namespace PintoresApp.Presentation.Menus;

/// <summary>
/// Contexto del patrón Strategy para el menú de Movimientos Artísticos.
/// Registra las estrategias disponibles y delega en MenuContext la presentación
/// y despacho del menú.
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
