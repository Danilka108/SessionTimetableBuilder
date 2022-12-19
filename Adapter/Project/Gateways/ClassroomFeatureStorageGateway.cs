using Adapter.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapter.Project.Gateways;

internal class ClassroomFeatureStorageGateway
    : BaseStorageGateway<Classroom, ClassroomSet>, IClassroomGateway
{
    public ClassroomFeatureStorageGateway(I storageProvider) : base
    (
        storageProvider.ProvideStorage(),
        new ClassroomFeatureSet.Helper()
    )
    {
    }

    protected override Task<AudienceSpecificity> ProduceModelByEntity
    (
        ClassroomFeatureSet entity,
        CancellationToken token
    )
    {
        return Task.FromResult
        (
            new AudienceSpecificity(entity.Description)
        );
    }
}