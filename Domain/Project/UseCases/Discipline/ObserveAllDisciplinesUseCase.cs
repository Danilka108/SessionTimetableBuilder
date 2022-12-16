using System.Reactive.Linq;
using Domain.Project.Repositories;

namespace Domain.Project.UseCases.Discipline;

public class ObserveAllDisciplinesUseCase
{
    private readonly IDisciplineRepository _repository;

    public ObserveAllDisciplinesUseCase(IDisciplineRepository repository)
    {
        _repository = repository;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.Discipline>>> Handle()
    {
        return _repository
            .ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Models.Discipline>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Models.Discipline>>>
                    (new ObserveAllDisciplinesException("Failed to get all disciplines.", e))
            );
    }
}

public class ObserveAllDisciplinesException : Exception
{
    internal ObserveAllDisciplinesException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}