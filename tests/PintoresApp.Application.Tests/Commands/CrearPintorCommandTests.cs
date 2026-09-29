using Moq;
using PintoresApp.Application.Commands;
using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Exceptions;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Tests.Commands;

public class CrearPintorCommandTests
{
    private readonly Mock<IPintorRepository> _repo = new();
    private readonly CrearPintorCommand _command;

    public CrearPintorCommandTests()
    {
        _command = new CrearPintorCommand(_repo.Object);
    }

    [Fact]
    public async Task EjecutarAsync_DatosValidos_AgregaPintorConEsosDatos()
    {
        await _command.EjecutarAsync("Frida", "Kahlo", "Mexicana", new DateTime(1907, 7, 6));

        _repo.Verify(r => r.AgregarAsync(It.Is<Pintor>(p =>
            p.Nombre == "Frida" &&
            p.Apellido == "Kahlo" &&
            p.Nacionalidad == "Mexicana" &&
            p.FechaNacimiento == new DateTime(1907, 7, 6))), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_NombreVacio_LanzaDomainExceptionYNoAgrega()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            _command.EjecutarAsync("", "Kahlo", "Mexicana", new DateTime(1907, 7, 6)));

        _repo.Verify(r => r.AgregarAsync(It.IsAny<Pintor>()), Times.Never);
    }

    [Fact]
    public async Task EjecutarAsync_FechaFutura_LanzaDomainExceptionYNoAgrega()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            _command.EjecutarAsync("Frida", "Kahlo", "Mexicana", DateTime.Today.AddDays(1)));

        _repo.Verify(r => r.AgregarAsync(It.IsAny<Pintor>()), Times.Never);
    }
}