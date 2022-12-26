using Domain.Project;

namespace Application.Project.Gateways;

public interface IClassroomFeatureGateway
{
    Task<ClassroomFeature> Create(string description, CancellationToken token);

    Task Update(ClassroomFeature feature, CancellationToken token);

    Task Delete(ClassroomFeature feature, CancellationToken token);

    Task<ClassroomFeature> Read(int id, CancellationToken token);

    Task<IEnumerable<ClassroomFeature>> ReadAll(CancellationToken token);

    IObservable<ClassroomFeature> Observe(int id);

    IObservable<IEnumerable<ClassroomFeature>> ObserveAll();
}

public class ClassroomFeatureGatewayException : Exception
{
    public ClassroomFeatureGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public ClassroomFeatureGatewayException(string message) : base(message)
    {
    }
}