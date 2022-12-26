using Application.Project.Gateways;

namespace Application.Project.UseCases.Classroom;

public class DeleteClassroomUseCase
{
    private readonly IClassroomGateway _gateway;
    
    private readonly IExamGateway _examGateway;

    public DeleteClassroomUseCase(IClassroomGateway gateway, IExamGateway examGateway)
    {
        _gateway = gateway;
        _examGateway = examGateway;
    }

    public async Task Handle(Domain.Project.Classroom classroom, CancellationToken token)
    {
        var allExams = await _examGateway.ReadAll(token);

        foreach (var exam in allExams)
        {
            if (exam.Classroom.Id == classroom.Id)
            {
                throw new ClassroomReferencedByExamException(exam);
            }
        }
        
        await _gateway.Delete(classroom, token);
    }
}

public class ClassroomReferencedByExamException : Exception
{
    public ClassroomReferencedByExamException(
        Domain.Project.Exam exam)
    {
        Exam = exam;
    }

    public Domain.Project.Exam Exam { get; }
}