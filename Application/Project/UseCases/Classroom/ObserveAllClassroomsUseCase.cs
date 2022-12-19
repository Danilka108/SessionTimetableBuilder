using System.Reactive.Linq;
using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.UseCases.classroom;

public class ObserveAllClassroomsUseCase
{
    private readonly IClassroomGateway _gateway;

    public ObserveAllClassroomsUseCase(IClassroomGateway gateway)
    {
        _gateway = gateway;
    }

    public IObservable<IEnumerable<Identified<Domain.Project.Classroom>>> Handle()
    {
        return _gateway
            .ObserveAll()
            .Catch<IEnumerable<Identified<Domain.Project.Classroom>>, Exception>
            (
                e => Observable.Throw<IEnumerable<Identified<Domain.Project.Classroom>>>
                    (new ObserveAllClassroomsException("Failed to get all classrooms", e))
            );
    }
}

public class ObserveAllClassroomsException : Exception
{
    internal ObserveAllClassroomsException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}