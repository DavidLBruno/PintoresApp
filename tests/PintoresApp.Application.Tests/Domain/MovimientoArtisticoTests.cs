using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;

namespace PintoresApp.Application.Tests.Domain;

public class MovimientoArtisticoTests
{
    [Fact]
    public void Constructor_DatosValidos_CreaMovimiento()
    {
        var movimiento = new MovimientoArtistico("Impresionismo", "Movimiento pictórico francés", "Francia");

        Assert.Equal("Impresionismo", movimiento.Nombre);
        Assert.Equal("Movimiento pictórico francés", movimiento.Descripcion);
        Assert.Equal("Francia", movimiento.PaisOrigen);
        Assert.NotEqual(Guid.Empty, movimiento.Id);
        Assert.Empty(movimiento.Pintores);
    }

    [Fact]
    public void Constructor_TextoConEspacios_LoRecorta()
    {
        var movimiento = new MovimientoArtistico("  Cubismo ", " Vanguardia artística  ", "  Francia  ");

        Assert.Equal("Cubismo", movimiento.Nombre);
        Assert.Equal("Vanguardia artística", movimiento.Descripcion);
        Assert.Equal("Francia", movimiento.PaisOrigen);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_NombreVacio_LanzaDomainException(string? nombre)
    {
        Assert.Throws<DomainException>(() =>
            new MovimientoArtistico(nombre!, "Descripción válida", "Francia"));
    }

    [Fact]
    public void Constructor_NombreConNumeros_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new MovimientoArtistico("Barroco 2", "Descripción", "Italia"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_DescripcionVacia_LanzaDomainException(string? descripcion)
    {
        Assert.Throws<DomainException>(() =>
            new MovimientoArtistico("Surrealismo", descripcion!, "Francia"));
    }

    [Fact]
    public void Constructor_DescripcionMuyLarga_LanzaDomainException()
    {
        var descripcionLarga = new string('A', MovimientoArtistico.LongitudMaximaDescripcion + 1);

        Assert.Throws<DomainException>(() =>
            new MovimientoArtistico("Surrealismo", descripcionLarga, "Francia"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_PaisOrigenVacio_LanzaDomainException(string? pais)
    {
        Assert.Throws<DomainException>(() =>
            new MovimientoArtistico("Surrealismo", "Descripción", pais!));
    }

    [Fact]
    public void Actualizar_DatosValidos_ModificaPropiedades()
    {
        var movimiento = new MovimientoArtistico("Impresionismo", "Descripción 1", "Francia");
        movimiento.Actualizar("Postimpresionismo", "Descripción 2", "Países Bajos");

        Assert.Equal("Postimpresionismo", movimiento.Nombre);
        Assert.Equal("Descripción 2", movimiento.Descripcion);
        Assert.Equal("Países Bajos", movimiento.PaisOrigen);
    }
}
