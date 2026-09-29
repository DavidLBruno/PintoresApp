using Moq;
using PintoresApp.Application.Commands;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Commands;

public class CrearMovimientoArtisticoCommandTests
{
    private readonly Mock<IMovimientoArtisticoRepository> _repo = new();
    private readonly CrearMovimientoArtisticoCommand _command;

    public CrearMovimientoArtisticoCommandTests()
    {
        _command = new CrearMovimientoArtisticoCommand(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_DatosValidos_AgregaMovimientoConEsosDatos()
    {
        var id = await _command.EjecutarAsync("Impresionismo", "Pintura al aire libre con luz", "Francia");

        Assert.NotEqual(Guid.Empty, id);
        _repo.Verify(r => r.AgregarAsync(It.Is<MovimientoArtistico>(m =>
            m.Nombre == "Impresionismo" &&
            m.Descripcion == "Pintura al aire libre con luz" &&
            m.PaisOrigen == "Francia")), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_NombreInvalido_LanzaDomainExceptionYNoAgrega()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            _command.EjecutarAsync("", "Descripción", "Francia"));

        _repo.Verify(r => r.AgregarAsync(It.IsAny<MovimientoArtistico>()), Times.Never);
    }
}
