using System.Reactive.Linq;
using Domain.Project.Repositories;

namespace Domain.Project.useCases.Teacher;

public class ObserveAllTeachersUseCase
{
    private readonly ITeacherRepository _repository;

    public ObserveAllTeachersUseCase(ITeacherRepository repository)
    {
        _repository = repository;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.Teacher>>> Handle()
    {
        return _repository
            .ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Models.Teacher>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Models.Teacher>>>
                    (new ObserveAllTeachersException("Failed to get all teachers.", e))
            );
    }
}

public class ObserveAllTeachersException : Exception
{
    internal ObserveAllTeachersException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}