using Adapter.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapter.Project.Gateways;

internal class DisciplineStorageRepository : BaseStorageGateway<Discipline, StorageDiscipline>,
    IDisciplineGateway
{
    private readonly IClassroomFeatureGateway _featureGateway;

    public DisciplineStorageRepository(Storage.Storage storage,
        IClassroomFeatureGateway featureGateway) : base(storage,
        new StorageDiscipline.Converter())
    {
        _featureGateway = featureGateway;
    }

    protected override async Task<Discipline> ProduceEntity(StorageDiscipline storageEntity,
        CancellationToken token)
    {
        var requirements = new List<Identified<ClassroomFeature>>();

        foreach (var linkedRequirement in storageEntity.Requirements)
        {
            var requirement = await _featureGateway.Read(linkedRequirement.Id, token);
            requirements.Add(requirement);
        }

        return new Discipline
        (
            storageEntity.Name,
            requirements
        );
    }
}