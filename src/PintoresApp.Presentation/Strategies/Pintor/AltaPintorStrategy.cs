using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;
using PintoresApp.Presentation.Helpers;

namespace PintoresApp.Presentation.Strategies.Pintor;

public class AltaPintorStrategy : IMenuStrategy
{
    private readonly IServiceScopeFactory _scopeFactory;

    public string Descripcion => "Alta";

    public AltaPintorStrategy(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EjecutarAsync()
    {
        Console.WriteLine("--- Alta de pintor ---");

        var nombre = ConsoleInput.LeerTexto("Nombre");
        var apellido = ConsoleInput.LeerTexto("Apellido");
        var nacionalidad = ConsoleInput.LeerTexto("Nacionalidad");
        var fecha = ConsoleInput.LeerFecha("Fecha de nacimiento");
        var movimientoId = await MovimientoSelector.SeleccionarAsync(_scopeFactory);

        using var scope = _scopeFactory.CreateScope();
        var crear = scope.ServiceProvider.GetRequiredService<CrearPintorCommand>();

        await crear.EjecutarAsync(nombre, apellido, nacionalidad, fecha, movimientoId);
        Console.WriteLine("\nPintor creado exitosamente.");
    }
}
