using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapters.Project.Gateways;

internal class ClassroomFeatureStorageGateway
    : BaseStorageGateway<ClassroomFeature, StorageClassroomFeature>, IClassroomFeatureGateway
{
    public ClassroomFeatureStorageGateway(Storage.Storage storage) : base
    (
        storage,
        new StorageClassroomFeature.Converter()
    )
    {
    }

    protected override async Task<ClassroomFeature> ProduceEntity
    (
        StorageClassroomFeature storageEntity,
        CancellationToken token
    )
    {
        return new ClassroomFeature(storageEntity.Description);
    }
}