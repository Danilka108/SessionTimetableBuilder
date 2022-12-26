using Domain.Project;

namespace Application.Project.Gateways;

public interface ILecturerGateway
{
    Task<Lecturer> Create(string name, string surname, string patronymic,
        IEnumerable<Discipline> disciplines,
        CancellationToken token);

    Task Update(Lecturer lecturer, CancellationToken token);

    Task Delete(Lecturer lecturer, CancellationToken token);

    Task<Lecturer> Read(int id, CancellationToken token);

    Task<IEnumerable<Lecturer>> ReadAll(CancellationToken token);

    IObservable<Lecturer> Observe(int id);

    IObservable<IEnumerable<Lecturer>> ObserveAll();
}

public class LecturerGatewayException : Exception
{
    public LecturerGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public LecturerGatewayException(string message) : base(message)
    {
    }
}