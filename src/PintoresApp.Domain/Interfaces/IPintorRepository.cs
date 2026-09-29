using PintoresApp.Domain.Entities;

namespace PintoresApp.Domain.Interfaces;

public interface IPintorRepository
{
    Task<IEnumerable<Pintor>> ObtenerTodosAsync();
    Task<Pintor?> ObtenerPorIdAsync(Guid id);
    Task AgregarAsync(Pintor pintor);
    Task ActualizarAsync(Pintor pintor);
    Task EliminarAsync(Pintor pintor);
}