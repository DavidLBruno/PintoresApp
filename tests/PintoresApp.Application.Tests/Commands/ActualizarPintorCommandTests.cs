using Moq;
using PintoresApp.Application.Commands;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Commands;

public class ActualizarPintorCommandTests
{
    private readonly Mock<IPintorRepository> _repo = new();
    private readonly ActualizarPintorCommand _command;

    public ActualizarPintorCommandTests()
    {
        _command = new ActualizarPintorCommand(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_PintorExiste_ActualizaDatosYGuarda()
    {
        var pintor = new Pintor("Diego", "Rivera", "Mexicana", new DateTime(1886, 12, 8));
        var id = pintor.Id;
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(pintor);

        await _command.EjecutarAsync(id, "Diego Maria", "Rivera Barrientos", "Mexicana", new DateTime(1886, 12, 8));

        Assert.Equal("Diego Maria", pintor.Nombre);
        Assert.Equal("Rivera Barrientos", pintor.Apellido);
        _repo.Verify(r => r.ActualizarAsync(pintor), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_PintorNoExiste_LanzaExceptionYNoActualiza()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync((Pintor?)null);

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _command.EjecutarAsync(id, "Diego", "Rivera", "Mexicana", new DateTime(1886, 12, 8)));

        Assert.Contains("No existe", ex.Message);
        _repo.Verify(r => r.ActualizarAsync(It.IsAny<Pintor>()), Times.Never);
    }

    [Fact]
    public async Task EjecutarAsync_DatosInvalidos_LanzaDomainExceptionYNoActualiza()
    {
        var pintor = new Pintor("Diego", "Rivera", "Mexicana", new DateTime(1886, 12, 8));
        var id = pintor.Id;
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(pintor);

        await Assert.ThrowsAsync<DomainException>(() =>
            _command.EjecutarAsync(id, "", "Rivera", "Mexicana", new DateTime(1886, 12, 8)));

        _repo.Verify(r => r.ActualizarAsync(It.IsAny<Pintor>()), Times.Never);
    }
}
