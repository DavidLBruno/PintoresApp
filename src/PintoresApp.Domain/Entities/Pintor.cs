using PintoresApp.Domain.Exceptions;

namespace PintoresApp.Domain.Entities;

public class Pintor
{
    public const int LongitudMaximaTexto = 100;

    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Nacionalidad { get; private set; } = string.Empty;
    public DateTime FechaNacimiento { get; private set; }
    public Guid? MovimientoArtisticoId { get; private set; }
    public MovimientoArtistico? MovimientoArtistico { get; private set; }

    // Requerido por EF Core para materializar desde la base. No usar.
    private Pintor() { }

    public Pintor(string nombre, string apellido, string nacionalidad, DateTime fechaNacimiento, Guid? movimientoArtisticoId = null)
    {
        Id = Guid.NewGuid();
        Actualizar(nombre, apellido, nacionalidad, fechaNacimiento, movimientoArtisticoId);
    }

    public void Actualizar(string nombre, string apellido, string nacionalidad, DateTime fechaNacimiento, Guid? movimientoArtisticoId = null)
    {
        Nombre = ValidarTextoSinNumeros(nombre, nameof(Nombre));
        Apellido = ValidarTextoSinNumeros(apellido, nameof(Apellido));
        Nacionalidad = ValidarTexto(nacionalidad, nameof(Nacionalidad));
        FechaNacimiento = ValidarFechaNacimiento(fechaNacimiento);
        MovimientoArtisticoId = movimientoArtisticoId;
    }

    public void AsignarMovimiento(Guid? movimientoArtisticoId)
    {
        MovimientoArtisticoId = movimientoArtisticoId;
    }

    private static string ValidarTexto(string valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException($"{campo} es obligatorio.");

        valor = valor.Trim();

        if (valor.Length > LongitudMaximaTexto)
            throw new DomainException($"{campo} no puede superar los {LongitudMaximaTexto} caracteres.");

        return valor;
    }

    private static string ValidarTextoSinNumeros(string valor, string campo)
    {
        valor = ValidarTexto(valor, campo);

        if (valor.Any(char.IsDigit))
            throw new DomainException($"{campo} no puede contener números.");

        return valor;
    }

    private static DateTime ValidarFechaNacimiento(DateTime fecha)
    {
        if (fecha.Date > DateTime.Today)
            throw new DomainException("La fecha de nacimiento no puede ser futura.");

        if (fecha.Year < 1200)
            throw new DomainException("La fecha de nacimiento no es válida.");

        return fecha.Date;
    }
}