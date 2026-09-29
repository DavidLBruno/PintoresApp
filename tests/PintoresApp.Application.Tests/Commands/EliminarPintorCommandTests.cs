using Moq;
using PintoresApp.Application.Commands;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Commands;

public class EliminarPintorCommandTests
{
    private readonly Mock<IPintorRepository> _repo = new();
    private readonly EliminarPintorCommand _command;

    public EliminarPintorCommandTests()
    {
        _command = new EliminarPintorCommand(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_PintorExiste_LoElimina()
    {
        var pintor = new Pintor("Claude", "Monet", "Francesa", new DateTime(1840, 11, 14));
        var id = pintor.Id;
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(pintor);

        await _command.EjecutarAsync(id);

        _repo.Verify(r => r.EliminarAsync(pintor), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_PintorNoExiste_LanzaExceptionYNoElimina()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync((Pintor?)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _command.EjecutarAsync(id));

        Assert.Contains("No existe", ex.Message);
        _repo.Verify(r => r.EliminarAsync(It.IsAny<Pintor>()), Times.Never);
    }
}