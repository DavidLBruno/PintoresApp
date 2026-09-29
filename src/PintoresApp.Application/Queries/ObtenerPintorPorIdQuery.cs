using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Queries;

public class ObtenerPintorPorIdQuery
{
    private readonly IPintorRepository _repository;

    public ObtenerPintorPorIdQuery(IPintorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Pintor?> EjecutarAsync(Guid id)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }


}