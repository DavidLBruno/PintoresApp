using Moq;
using PintoresApp.Application.Queries;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Queries;

public class ObtenerPintoresQueryTests
{
    private readonly Mock<IPintorRepository> _repo = new();
    private readonly ObtenerPintoresQuery _query;

    public ObtenerPintoresQueryTests()
    {
        _query = new ObtenerPintoresQuery(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_HayPintores_DevuelveTodos()
    {
        var pintores = new List<Pintor>
        {
            new("Frida", "Kahlo", "Mexicana", new DateTime(1907, 7, 6)),
            new("Claude", "Monet", "Francesa", new DateTime(1840, 11, 14))
        };
        _repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(pintores);

        var resultado = (await _query.EjecutarAsync()).ToList();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, p => p.Apellido == "Kahlo");
        Assert.Contains(resultado, p => p.Apellido == "Monet");
    }

    [Fact]
    public async Task EjecutarAsync_SinPintores_DevuelveListaVacia()
    {
        _repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Pintor>());

        var resultado = await _query.EjecutarAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task EjecutarAsync_LlamaAlRepositorioUnaVez()
    {
        _repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Pintor>());

        await _query.EjecutarAsync();

        _repo.Verify(r => r.ObtenerTodosAsync(), Times.Once);
    }
}