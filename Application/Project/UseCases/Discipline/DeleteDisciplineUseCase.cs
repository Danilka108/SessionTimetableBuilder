using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

public class DeleteDisciplineUseCase
{
    private readonly IDisciplineGateway _gateway;

    private readonly ILecturerGateway _lecturerGateway;

    private readonly IGroupGateway _groupGateway;

    private readonly IExamGateway _examGateway;

    public DeleteDisciplineUseCase(
        IDisciplineGateway gateway,
        ILecturerGateway lecturerGateway,
        IGroupGateway groupGateway,
        IExamGateway examGateway
    )
    {
        _gateway = gateway;
        _lecturerGateway = lecturerGateway;
        _groupGateway = groupGateway;
        _examGateway = examGateway;
    }

    public async Task Handle(Domain.Project.Discipline discipline, CancellationToken token)
    {

        await CheckGroupsToReferences(discipline, token);
        await CheckLecturersToReferences(discipline, token);
        await CheckExamsToReferences(discipline, token);

        await _gateway.Delete(discipline, token);
    }

    private async Task CheckGroupsToReferences(Domain.Project.Discipline discipline,
        CancellationToken token)
    {
        var allGroups = await _groupGateway.ReadAll(token);

        foreach (var group in allGroups)
        {
            var contains = group.ContainsDiscipline(discipline);

            if (contains)
            {
                throw new DisciplineReferencedByGroupException(group);
            }
        }
    }
    
    private async Task CheckLecturersToReferences(Domain.Project.Discipline discipline,
        CancellationToken token)
    {
        var allLecturers = await _lecturerGateway.ReadAll(token);

        foreach (var lecturer in allLecturers)
        {
            var contains = lecturer.ContainsDiscipline(discipline);

            if (contains)
            {
                throw new DisciplineReferencedByLecturerException(lecturer);
            }
        }
    }
    
    private async Task CheckExamsToReferences(Domain.Project.Discipline discipline,
        CancellationToken token)
    {
        var allExams = await _examGateway.ReadAll(token);

        foreach (var exam in allExams)
        {
            if (exam.Discipline.Id == discipline.Id)
            {
                throw new DisciplineReferencedByExamException(exam);
            }
        }
    }
}

public class DisciplineReferencedByLecturerException : Exception
{
    public DisciplineReferencedByLecturerException(
        Domain.Project.Lecturer lecturer)
    {
        Lecturer = lecturer;
    }

    public Domain.Project.Lecturer Lecturer { get; }
}

public class DisciplineReferencedByGroupException : Exception
{
    public DisciplineReferencedByGroupException(
        Domain.Project.Group group)
    {
        Group = group;
    }

    public Domain.Project.Group Group { get; }
}

public class DisciplineReferencedByExamException : Exception
{
    public DisciplineReferencedByExamException(
        Domain.Project.Exam exam)
    {
        Exam = exam;
    }

    public Domain.Project.Exam Exam { get; }
}