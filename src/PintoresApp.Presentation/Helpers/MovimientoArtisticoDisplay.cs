using PintoresApp.Domain.Entities;

namespace PintoresApp.Presentation.Helpers;

/// <summary>
/// Helper para mostrar en consola los datos de un MovimientoArtístico.
/// Centraliza la presentación evitando duplicación entre estrategias.
/// </summary>
public static class MovimientoArtisticoDisplay
{
    public static void Mostrar(MovimientoArtistico m)
    {
        Console.WriteLine($"\nId: {m.Id}");
        Console.WriteLine($"Nombre: {m.Nombre}");
        Console.WriteLine($"País de origen: {m.PaisOrigen}");
        Console.WriteLine($"Descripción: {m.Descripcion}");
    }
}
