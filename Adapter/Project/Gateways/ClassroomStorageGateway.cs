using Adapter.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapter.Project.Gateways;

internal class ClassroomStorageGateway : BaseStorageGateway<Classroom, StorageClassroom>,
    IClassroomGateway

{
    private readonly IClassroomFeatureGateway _featureGateway;

    public ClassroomStorageGateway(Storage.Storage storage,
        IClassroomFeatureGateway featureGateway) : base(storage,
        new StorageClassroom.Converter())
    {
        _featureGateway = featureGateway;
    }

    protected override async Task<Classroom> ProduceEntity(StorageClassroom storageEntity,
        CancellationToken token)
    {
        var features = new List<Identified<ClassroomFeature>>();

        foreach (var linkedFeature in storageEntity.Features)
        {
            var feature = await _featureGateway.Read(linkedFeature.Id, token);
            features.Add(feature);
        }

        return new Classroom
        (
            storageEntity.Number,
            storageEntity.Capacity,
            features
        );
    }
}