using System.Globalization;
using System.Text;

namespace PintoresApp.Presentation.Helpers;

public static class ConsoleInput
{
    public static string LeerTexto(string etiqueta, string? valorActual = null)
    {
        Console.Write(valorActual is null ? $"{etiqueta}: " : $"{etiqueta} [{valorActual}]: ");
        var entrada = Console.ReadLine();

        return string.IsNullOrWhiteSpace(entrada) && valorActual is not null
            ? valorActual
            : entrada ?? string.Empty;
    }

    public static int LeerEntero(string etiqueta)
    {
        while (true)
        {
            Console.Write($"{etiqueta}: ");
            if (int.TryParse(Console.ReadLine(), out var valor))
                return valor;

            Console.WriteLine("Ingresá un número válido.");
        }
    }

    public static Guid LeerGuid(string etiqueta)
    {
        while (true)
        {
            Console.Write($"{etiqueta}: ");
            if (Guid.TryParse(Console.ReadLine(), out var valor))
                return valor;

            Console.WriteLine("Ingresá un GUID válido.");
        }
    }

    public static DateTime LeerFecha(string etiqueta, DateTime? valorActual = null)
    {
        while (true)
        {
            Console.Write(valorActual is null
                ? $"{etiqueta} (dd/MM/yyyy): "
                : $"{etiqueta} (dd/MM/yyyy) [{valorActual:dd/MM/yyyy}]: ");

            if (Console.IsInputRedirected)
            {
                var entrada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entrada) && valorActual is not null)
                    return valorActual.Value;

                if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var fechaRedirect))
                    return fechaRedirect;

                Console.WriteLine("Formato inválido. Ejemplo: 25/10/1881");
                continue;
            }

            var buffer = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (buffer.Length == 0 && valorActual is not null)
                    {
                        Console.WriteLine();
                        return valorActual.Value;
                    }

                    if (buffer.Length == 10)
                    {
                        Console.WriteLine();
                        if (DateTime.TryParseExact(buffer.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out var fecha))
                        {
                            return fecha;
                        }

                        Console.WriteLine("Fecha inválida. Ejemplo: 25/10/1881");
                        break;
                    }

                    if (buffer.Length > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Fecha incompleta. Debe ingresar el formato dd/MM/yyyy.");
                        break;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        if (buffer[^1] == '/')
                        {
                            buffer.Remove(buffer.Length - 1, 1);
                            Console.Write("\b \b");
                            if (buffer.Length > 0)
                            {
                                buffer.Remove(buffer.Length - 1, 1);
                                Console.Write("\b \b");
                            }
                        }
                        else
                        {
                            buffer.Remove(buffer.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                    }
                }
                else if (char.IsDigit(keyInfo.KeyChar))
                {
                    if (buffer.Length < 10)
                    {
                        buffer.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar);

                        if (buffer.Length == 2 || buffer.Length == 5)
                        {
                            buffer.Append('/');
                            Console.Write('/');
                        }
                    }
                }
                else if (keyInfo.KeyChar == '/' || keyInfo.KeyChar == '-')
                {
                    if (buffer.Length == 1)
                    {
                        char d = buffer[0];
                        buffer.Clear();
                        buffer.Append('0').Append(d).Append('/');
                        Console.Write($"\b \b0{d}/");
                    }
                    else if (buffer.Length == 4)
                    {
                        char m = buffer[3];
                        buffer.Remove(3, 1);
                        buffer.Append('0').Append(m).Append('/');
                        Console.Write($"\b \b0{m}/");
                    }
                }
            }
        }
    }

    public static bool Confirmar(string mensaje)
    {
        Console.Write($"{mensaje} (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public static void Pausa()
    {
        Console.WriteLine("\nPresioná una tecla para continuar...");
        Console.ReadKey(true);
    }
}