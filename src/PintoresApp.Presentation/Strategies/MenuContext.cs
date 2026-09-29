using PintoresApp.Domain.Exceptions;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies;

/// <summary>
/// Contexto del patrón Strategy.
/// Mantiene una lista de estrategias disponibles, presenta el menú dinámicamente
/// a partir de ellas y delega la ejecución en la estrategia seleccionada.
/// </summary>
public class MenuContext
{
    private readonly string _titulo;
    private readonly IReadOnlyList<IMenuStrategy> _estrategias;

    public MenuContext(string titulo, IEnumerable<IMenuStrategy> estrategias)
    {
        _titulo = titulo;
        _estrategias = estrategias.ToList();
    }

    public async Task EjecutarAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== {_titulo} =====");

            for (int i = 0; i < _estrategias.Count; i++)
                Console.WriteLine($"{i + 1}. {_estrategias[i].Descripcion}");

            int opcionSalir = _estrategias.Count + 1;
            Console.WriteLine($"{opcionSalir}. Volver");
            Console.Write("\nOpción: ");

            var entrada = Console.ReadLine()?.Trim();
            Console.WriteLine();

            if (entrada == opcionSalir.ToString() || entrada == "0") return;

            if (!int.TryParse(entrada, out int opcion) ||
                opcion < 1 || opcion > _estrategias.Count)
            {
                Console.WriteLine("Opción inválida.");
                ConsoleInput.Pausa();
                continue;
            }

            try
            {
                await _estrategias[opcion - 1].EjecutarAsync();
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
}
