using System.Reactive.Linq;
using Application.Project.Gateways;
using Domain.Project.Repositories;

namespace Application.Project.useCases.Lecturer;

public class ObserveAllLecturersUseCase
{
    private readonly ILecturerGateway _gateway;

    public ObserveAllLecturersUseCase(ITeacherGateWay gateway)
    {
        _gateway = gateway;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.Teacher>>> Handle()
    {
        return _gateway
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