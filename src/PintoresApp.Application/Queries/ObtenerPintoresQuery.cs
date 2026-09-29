using PintoresApp.Domain.Entities;
using PintoresApp.Domain.Interfaces;

namespace PintoresApp.Application.Queries;

public class ObtenerPintoresQuery
{
    private readonly IPintorRepository _repository;

    public ObtenerPintoresQuery(IPintorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Pintor>> EjecutarAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }


}