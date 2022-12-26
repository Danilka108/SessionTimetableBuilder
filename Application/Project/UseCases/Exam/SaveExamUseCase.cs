using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.UseCases.Exam;

public class SaveExamUseCase
{
    private readonly IExamGateway _gateway;

    public SaveExamUseCase(IExamGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Domain.Project.Exam> Handle(
        Lecturer lecturer,
        Domain.Project.Discipline discipline,
        Domain.Project.Classroom classroom,
        Domain.Project.Group group,
        DateTime startTime, int? id, CancellationToken token
    )
    {
        if (!lecturer.ContainsDiscipline(discipline))
        {
            throw new LecturerDoesNotAcceptDiscipline();
        }

        if (!group.ContainsDiscipline(discipline))
        {
            throw new GroupDoesNotStudyDiscipline();
        }

        if (!classroom.MeetsDisciplineRequirements(discipline))
        {
            throw new ClassroomDoesNotMeetsRequirements();
        }

        await CheckExamToOriginality(id, discipline, group, token);

        if (id is { } notNullId)
        {
            var exam = new Domain.Project.Exam(notNullId,
                lecturer,
                group,
                discipline,
                classroom,
                startTime
            );
            await _gateway.Update(exam, token);
            return exam;
        }

        return await _gateway.Create(lecturer,
            group,
            discipline,
            classroom,
            startTime, token
        );
    }

    async Task CheckExamToOriginality(int? id, Domain.Project.Discipline discipline,
        Domain.Project.Group group, CancellationToken token)
    {
        var allExams = await _gateway.ReadAll(token);


        foreach (var exam in allExams)
        {
            if (id == exam.Id) continue;

            if (exam.Group.Id == group.Id && exam.Discipline.Id == discipline.Id)
            {
                throw new SameExamAlreadyExists(exam);
            }
        }
    }
}

public class SameExamAlreadyExists : Exception
{
    public SameExamAlreadyExists(Domain.Project.Exam exam)
    {
        Exam = exam;
    }

    public Domain.Project.Exam Exam { get; }
}

public class ClassroomDoesNotMeetsRequirements : Exception
{
}

public class LecturerDoesNotAcceptDiscipline : Exception
{
}

public class GroupDoesNotStudyDiscipline : Exception
{
}