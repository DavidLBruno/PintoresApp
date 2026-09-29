namespace PintoresApp.Presentation.Strategies;

/// <summary>
/// Estrategia base para todas las operaciones de menú.
/// Cada operación concreta (Alta, Baja, Modificación, etc.) implementa esta interfaz.
/// </summary>
public interface IMenuStrategy
{
    string Descripcion { get; }
    Task EjecutarAsync();
}
