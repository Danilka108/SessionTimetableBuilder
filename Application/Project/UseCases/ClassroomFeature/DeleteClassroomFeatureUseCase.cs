using Application.Project.Gateways;

namespace Application.Project.UseCases.ClassroomFeature;

public class DeleteClassroomFeatureUseCase
{
    private readonly IClassroomFeatureGateway _featureGateway;
    private readonly IClassroomGateway _classroomGateway;
    private readonly IDisciplineGateway _disciplineGatway;

    public DeleteClassroomFeatureUseCase
    (IClassroomFeatureGateway featureGateway, IClassroomGateway classroomGateway,
        IDisciplineGateway disciplineGateway)
    {
        _featureGateway = featureGateway;
        _classroomGateway = classroomGateway;
        _disciplineGatway = disciplineGateway;
    }

    public async Task Handle(Domain.Project.ClassroomFeature feature, CancellationToken token)
    {
        await CheckForLackOfReferences(feature, token);
        await _featureGateway.Delete(feature, token);
    }

    private async Task CheckForLackOfReferences(
        Domain.Project.ClassroomFeature featureToDelete, CancellationToken token)
    {
        var classrooms = await _classroomGateway.ReadAll(token);
        var disciplines = await _disciplineGatway.ReadAll(token);

        foreach (var classroom in classrooms)
        {
            var contains = classroom.ContainsFeature(featureToDelete);

            if (contains)
            {
                throw new ClassroomFeatureReferencedByClassroomException(classroom);
            }
        }
        
        foreach (var discipline in disciplines)
        {
            var contains = discipline.ContainsRequirement(featureToDelete);

            if (contains)
            {
                throw new ClassroomFeatureReferencedByDisciplineException(discipline);
            }
        }
    }
}

public class ClassroomFeatureReferencedByClassroomException : Exception
{
    public Domain.Project.Classroom Classroom { get; }

    public ClassroomFeatureReferencedByClassroomException(Domain.Project.Classroom classroom)
    {
        Classroom = classroom;
    }
}

public class ClassroomFeatureReferencedByDisciplineException : Exception
{
    public Domain.Project.Discipline Discipline { get; }

    public ClassroomFeatureReferencedByDisciplineException(
        Domain.Project.Discipline discipline)
    {
        Discipline = discipline;
    }
}