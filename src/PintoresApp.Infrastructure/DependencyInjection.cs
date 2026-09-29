using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PintoresApp.Domain.Interfaces;
using PintoresApp.Infrastructure.Persistence;
using PintoresApp.Infrastructure.Repositories;

namespace PintoresApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IPintorRepository, PintorRepository>();
        services.AddScoped<IMovimientoArtisticoRepository, MovimientoArtisticoRepository>();
        return services;
    }

    public static IServiceProvider InicializarBaseDeDatos(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
        return provider;
    }
}