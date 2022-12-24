using Application.Project.Gateways;

namespace Application.Project.UseCases.ClassroomFeature;

public class DeleteClassroomFeatureUseCase
{
    private readonly IClassroomFeatureGateway _featureGateway;
    private readonly IClassroomGateway _classroomGateway;

    public DeleteClassroomFeatureUseCase
        (IClassroomFeatureGateway featureGateway, IClassroomGateway classroomGateway)
    {
        _featureGateway = featureGateway;
        _classroomGateway = classroomGateway;
    }

    public async Task Handle(Domain.Project.ClassroomFeature feature, CancellationToken token)
    {
        var classroomWithSameFeature = await SearchClassroomWithSameFeature(feature, token);

        if (classroomWithSameFeature is not null)
        {
            throw new ClassroomFeatureAlreadyLinkedByClassroomException(classroomWithSameFeature);
        }

        await _featureGateway.Delete(feature, token);
    }

    private async Task<Domain.Project.Classroom?> SearchClassroomWithSameFeature(
        Domain.Project.ClassroomFeature featureToDelete, CancellationToken token)
    {
        var classrooms = await _classroomGateway.ReadAll(token);

        foreach (var classroom in classrooms)
        {
            var sameFeature =
                classroom.Features.FirstOrDefault(feature => feature.Id == featureToDelete.Id);

            if (sameFeature is not null)
            {
                return classroom;
            }
        }

        return null;
    }
}

public class ClassroomFeatureAlreadyLinkedByClassroomException : Exception
{
    public Domain.Project.Classroom LinkedClassroom { get; }
    
    public ClassroomFeatureAlreadyLinkedByClassroomException(Domain.Project.Classroom linkedClassroom)
    {
        LinkedClassroom = linkedClassroom;
    }
}