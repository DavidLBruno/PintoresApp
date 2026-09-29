using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;

namespace PintoresApp.Application.Tests.Domain;

public class PintorTests
{
    private static readonly DateTime FechaValida = new(1881, 10, 25);

    // ---------- Constructor ----------

    [Fact]
    public void Constructor_DatosValidos_CreaPintor()
    {
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", FechaValida);

        Assert.Equal("Pablo", pintor.Nombre);
        Assert.Equal("Picasso", pintor.Apellido);
        Assert.Equal("Espanola", pintor.Nacionalidad);
        Assert.Equal(FechaValida, pintor.FechaNacimiento);
        Assert.NotEqual(Guid.Empty, pintor.Id);
    }

    [Fact]
    public void Constructor_TextoConEspacios_LoRecorta()
    {
        var pintor = new Pintor("  Pablo ", " Picasso ", " Espanola ", FechaValida);

        Assert.Equal("Pablo", pintor.Nombre);
        Assert.Equal("Picasso", pintor.Apellido);
        Assert.Equal("Espanola", pintor.Nacionalidad);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_NombreVacio_LanzaDomainException(string? nombre)
    {
        Assert.Throws<DomainException>(() =>
            new Pintor(nombre!, "Picasso", "Espanola", FechaValida));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_ApellidoVacio_LanzaDomainException(string? apellido)
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo", apellido!, "Espanola", FechaValida));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_NacionalidadVacia_LanzaDomainException(string? nacionalidad)
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo", "Picasso", nacionalidad!, FechaValida));
    }

    [Fact]
    public void Constructor_TextoDemasiadoLargo_LanzaDomainException()
    {
        var largo = new string('a', Pintor.LongitudMaximaTexto + 1);

        Assert.Throws<DomainException>(() =>
            new Pintor(largo, "Picasso", "Espanola", FechaValida));
    }

    [Fact]
    public void Constructor_TextoEnElLimite_CreaPintor()
    {
        var limite = new string('a', Pintor.LongitudMaximaTexto);

        var pintor = new Pintor(limite, "Picasso", "Espanola", FechaValida);

        Assert.Equal(Pintor.LongitudMaximaTexto, pintor.Nombre.Length);
    }

    [Fact]
    public void Constructor_FechaFutura_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo", "Picasso", "Espanola", DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void Constructor_FechaAnteriorA1200_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo", "Picasso", "Espanola", new DateTime(1100, 1, 1)));
    }

    [Fact]
    public void Constructor_FechaConHora_GuardaSoloLaFecha()
    {
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", new DateTime(1881, 10, 25, 15, 30, 0));

        Assert.Equal(new DateTime(1881, 10, 25), pintor.FechaNacimiento);
    }

    [Fact]
    public void Constructor_NombreVacio_MensajeIndicaElCampo()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Pintor("", "Picasso", "Espanola", FechaValida));

        Assert.Contains("Nombre", ex.Message);
    }

    [Fact]
    public void Constructor_NombreConNumeros_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo3", "Picasso", "Espanola", FechaValida));
    }

    [Fact]
    public void Constructor_ApellidoConNumeros_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new Pintor("Pablo", "Picasso7", "Espanola", FechaValida));
    }

    // ---------- Actualizar ----------

    [Fact]
    public void Actualizar_DatosValidos_ModificaPintor()
    {
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", FechaValida);

        pintor.Actualizar("Pablo Diego", "Ruiz Picasso", "Espanola", new DateTime(1881, 10, 26));

        Assert.Equal("Pablo Diego", pintor.Nombre);
        Assert.Equal("Ruiz Picasso", pintor.Apellido);
        Assert.Equal(new DateTime(1881, 10, 26), pintor.FechaNacimiento);
    }

    [Fact]
    public void Actualizar_DatosInvalidos_LanzaDomainException()
    {
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", FechaValida);

        Assert.Throws<DomainException>(() =>
            pintor.Actualizar("", "Picasso", "Espanola", FechaValida));
    }

    [Fact]
    public void Constructor_ConMovimientoArtisticoId_LoAsigna()
    {
        var movimientoId = Guid.NewGuid();
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", FechaValida, movimientoId);

        Assert.Equal(movimientoId, pintor.MovimientoArtisticoId);
    }

    [Fact]
    public void AsignarMovimiento_ModificaMovimientoArtisticoId()
    {
        var pintor = new Pintor("Pablo", "Picasso", "Espanola", FechaValida);
        var movimientoId = Guid.NewGuid();

        pintor.AsignarMovimiento(movimientoId);

        Assert.Equal(movimientoId, pintor.MovimientoArtisticoId);

        pintor.AsignarMovimiento(null);
        Assert.Null(pintor.MovimientoArtisticoId);
    }
}
