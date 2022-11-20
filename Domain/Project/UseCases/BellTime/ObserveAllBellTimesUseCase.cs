using System.Reactive.Linq;
using Domain.Project.Repositories;

namespace Domain.Project.UseCases.BellTime;

public class ObserveAllBellTimesUseCase
{
    private readonly IBellTimeRepository _repository;

    public ObserveAllBellTimesUseCase(IBellTimeRepository repository)
    {
        _repository = repository;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.BellTime>>> Handle()
    {
        return _repository
            .ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Models.BellTime>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Models.BellTime>>>
                    (new ObserveAllBellTimesException("Failed to get all bell times.", e))
            );
    }
}

public class ObserveAllBellTimesException : Exception
{
    internal ObserveAllBellTimesException(string msg, Exception innerException)
        : base(msg, innerException)
    {
    }
}