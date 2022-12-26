using Domain.Project;

namespace Application.Project.Gateways;

public interface IExamGateway
{
    Task<Exam> Create(
        Lecturer lecturer,
        Group group,
        Discipline discipline,
        Classroom classroom,
        DateTime startTime,
        CancellationToken token
    );

    Task Update(Exam exam, CancellationToken token);

    Task Delete(Exam exam, CancellationToken token);

    Task<Exam> Read(int id, CancellationToken token);

    Task<IEnumerable<Exam>> ReadAll(CancellationToken token);

    IObservable<Exam> Observe(int id);

    IObservable<IEnumerable<Exam>> ObserveAll();
}

public class ExamGatewayException : Exception
{
    public ExamGatewayException(string message, Exception innerException) : base(
        message, innerException)
    {
    }

    public ExamGatewayException(string message) : base(message)
    {
    }
}