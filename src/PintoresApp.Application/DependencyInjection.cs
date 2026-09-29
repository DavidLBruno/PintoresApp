using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Application.Commands;
using PintoresApp.Application.Queries;

namespace PintoresApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CrearPintorCommand>();
        services.AddScoped<ActualizarPintorCommand>();
        services.AddScoped<EliminarPintorCommand>();
        services.AddScoped<ObtenerPintoresQuery>();
        services.AddScoped<ObtenerPintorPorIdQuery>();

        services.AddScoped<CrearMovimientoArtisticoCommand>();
        services.AddScoped<ActualizarMovimientoArtisticoCommand>();
        services.AddScoped<EliminarMovimientoArtisticoCommand>();
        services.AddScoped<ObtenerMovimientosArtisticosQuery>();
        services.AddScoped<ObtenerMovimientoArtisticoPorIdQuery>();
        return services;
    }
}