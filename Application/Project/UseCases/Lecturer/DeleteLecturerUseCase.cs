using Application.Project.Gateways;

namespace Application.Project.useCases.Lecturer;

public class DeleteLecturerUseCase
{
    private readonly ILecturerGateway _gateway;

    private readonly IExamGateway _examGateway;

    public DeleteLecturerUseCase(ILecturerGateway gateway, IExamGateway examGateway)
    {
        
        _gateway = gateway;
        _examGateway = examGateway;
    }

    public async Task Handle(Domain.Project.Lecturer lecturer, CancellationToken token)
    {
        var allExams = await _examGateway.ReadAll(token);

        foreach (var exam in allExams)
        {
            if (exam.Lecturer.Id == lecturer.Id)
            {
                throw new LecturerReferencedByExamException(exam);
            }
        }
        
        await _gateway.Delete(lecturer, token);
    }
}

public class LecturerReferencedByExamException : Exception
{
    public LecturerReferencedByExamException(
        Domain.Project.Exam exam)
    {
        Exam = exam;
    }

    public Domain.Project.Exam Exam { get; }
}
