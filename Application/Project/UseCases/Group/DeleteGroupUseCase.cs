using Application.Project.Gateways;

namespace Application.Project.UseCases.Group;

public class DeleteGroupUseCase
{
    private readonly IGroupGateway _gateway;

    private readonly IExamGateway _examGateway; 
    
    public DeleteGroupUseCase(IGroupGateway gateway, IExamGateway examGateway)
    {
        _gateway = gateway;
        _examGateway = examGateway;
    }

    public async Task Handle(Domain.Project.Group group, CancellationToken token)
    {
        
        var allExams = await _examGateway.ReadAll(token);

        foreach (var exam in allExams)
        {
            if (exam.Group.Id == group.Id)
            {
                throw new GroupReferencedByExamException(exam);
            }
        }
        
        await _gateway.Delete(group, token);
    }
}

public class GroupReferencedByExamException : Exception
{
    public GroupReferencedByExamException(
        Domain.Project.Exam exam)
    {
        Exam = exam;
    }

    public Domain.Project.Exam Exam { get; }
}
