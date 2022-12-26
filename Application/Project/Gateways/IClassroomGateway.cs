using Domain.Project;

namespace Application.Project.Gateways;

public interface IClassroomGateway
{
    Task<Classroom> Create(int number, int capacity, IEnumerable<ClassroomFeature> features,
        CancellationToken token);

    Task Update(Classroom classroom, CancellationToken token);

    Task Delete(Classroom classroom, CancellationToken token);

    Task<Classroom> Read(int id, CancellationToken token);

    Task<IEnumerable<Classroom>> ReadAll(CancellationToken token);

    IObservable<Classroom> Observe(int id);

    IObservable<IEnumerable<Classroom>> ObserveAll();
}

public class ClassroomGatewayException : Exception
{
    public ClassroomGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public ClassroomGatewayException(string message) : base(message)
    {
    }
}