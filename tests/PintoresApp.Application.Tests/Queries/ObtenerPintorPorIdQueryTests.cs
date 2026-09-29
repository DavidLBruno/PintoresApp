using Moq;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Queries;

public class ObtenerPintorPorIdQueryTests
{
    private readonly Mock<IPintorRepository> _repo = new();
    private readonly ObtenerPintorPorIdQuery _query;

    public ObtenerPintorPorIdQueryTests()
    {
        _query = new ObtenerPintorPorIdQuery(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_PintorExiste_LoDevuelve()
    {
        var pintor = new Pintor("Frida", "Kahlo", "Mexicana", new DateTime(1907, 7, 6));
        var id = pintor.Id;
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(pintor);

        var resultado = await _query.EjecutarAsync(id);

        Assert.Same(pintor, resultado);
    }

    [Fact]
    public async Task EjecutarAsync_PintorNoExiste_DevuelveNull()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync((Pintor?)null);

        var resultado = await _query.EjecutarAsync(id);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task EjecutarAsync_BuscaPorElIdRecibido()
    {
        var id = Guid.NewGuid();

        await _query.EjecutarAsync(id);

        _repo.Verify(r => r.ObtenerPorIdAsync(id), Times.Once);
    }
}
