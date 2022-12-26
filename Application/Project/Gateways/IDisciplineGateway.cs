using Domain.Project;

namespace Application.Project.Gateways;

public interface IDisciplineGateway
{
    Task<Discipline> Create(string name, IEnumerable<ClassroomFeature> classroomRequirements,
        CancellationToken token);

    Task Update(Discipline discipline, CancellationToken token);

    Task Delete(Discipline discipline, CancellationToken token);

    Task<Discipline> Read(int id, CancellationToken token);

    Task<IEnumerable<Discipline>> ReadAll(CancellationToken token);

    IObservable<Discipline> Observe(int id);

    IObservable<IEnumerable<Discipline>> ObserveAll();
}

public class DisciplineGatewayException : Exception
{
    public DisciplineGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public DisciplineGatewayException(string message) : base(message)
    {
    }
}