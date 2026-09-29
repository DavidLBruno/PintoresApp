using PintoresApp.Domain.Exceptions;

namespace PintoresApp.Domain.Entities;

public class MovimientoArtistico
{
    public const int LongitudMaximaTexto = 100;
    public const int LongitudMaximaDescripcion = 500;

    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string PaisOrigen { get; private set; } = string.Empty;

    private readonly List<Pintor> _pintores = [];
    public IReadOnlyCollection<Pintor> Pintores => _pintores.AsReadOnly();

    // Requerido por EF Core para materializar desde la base. No usar.
    private MovimientoArtistico() { }

    public MovimientoArtistico(string nombre, string descripcion, string paisOrigen)
    {
        Id = Guid.NewGuid();
        Actualizar(nombre, descripcion, paisOrigen);
    }

    public void Actualizar(string nombre, string descripcion, string paisOrigen)
    {
        Nombre = ValidarTextoSinNumeros(nombre, nameof(Nombre));
        Descripcion = ValidarDescripcion(descripcion);
        PaisOrigen = ValidarTextoSinNumeros(paisOrigen, nameof(PaisOrigen));
    }

    private static string ValidarTexto(string valor, string campo, int longitudMaxima = LongitudMaximaTexto)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException($"{campo} es obligatorio.");

        valor = valor.Trim();

        if (valor.Length > longitudMaxima)
            throw new DomainException($"{campo} no puede superar los {longitudMaxima} caracteres.");

        return valor;
    }

    private static string ValidarTextoSinNumeros(string valor, string campo)
    {
        valor = ValidarTexto(valor, campo);

        if (valor.Any(char.IsDigit))
            throw new DomainException($"{campo} no puede contener números.");

        return valor;
    }

    private static string ValidarDescripcion(string descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new DomainException("Descripción es obligatoria.");

        descripcion = descripcion.Trim();

        if (descripcion.Length > LongitudMaximaDescripcion)
            throw new DomainException($"Descripción no puede superar los {LongitudMaximaDescripcion} caracteres.");

        return descripcion;
    }
}
