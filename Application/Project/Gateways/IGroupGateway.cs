using Domain.Project;

namespace Application.Project.Gateways;

public interface IGroupGateway
{
    Task<Group> Create(string name, int studentsNumber, IEnumerable<Discipline> disciplines,
        CancellationToken token);

    Task Update(Group group, CancellationToken token);

    Task Delete(Group group, CancellationToken token);

    Task<Group> Read(int id, CancellationToken token);

    Task<IEnumerable<Group>> ReadAll(CancellationToken token);

    IObservable<Group> Observe(int id);

    IObservable<IEnumerable<Group>> ObserveAll();
}

public class GroupGatewayException : Exception
{
    public GroupGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public GroupGatewayException(string message) : base(message)
    {
    }
}
