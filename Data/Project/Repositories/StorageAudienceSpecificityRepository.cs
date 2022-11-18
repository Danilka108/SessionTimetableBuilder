using Data.Project.Entities;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class StorageAudienceSpecificityRepository
    : BaseStorageRepository<AudienceSpecificityEntity, AudienceSpecificity>,
        IAudienceSpecificityRepository
{
    public StorageAudienceSpecificityRepository(StorageProvider storageProvider) : base
    (
        storageProvider.ProvideStorage(),
        new AudienceSpecificityEntity.Helper()
    )
    {
    }

    protected override Task<AudienceSpecificity> ProduceModelByEntity
    (
        AudienceSpecificityEntity entity,
        CancellationToken token
    )
    {
        return Task.FromResult
        (
            new AudienceSpecificity(entity.Description)
        );
    }
}