using Data.Project.Entities;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class StorageBellTimeRepository : BaseStorageRepository<BellTimeEntity, BellTime>,
    IBellTimeRepository
{
    public StorageBellTimeRepository(StorageProvider storageProvider) : base
    (
        storageProvider.ProvideStorage(),
        new BellTimeEntity.Helper()
    )
    {
    }

    protected override Task<BellTime> ProduceModelByEntity
    (
        BellTimeEntity entity,
        CancellationToken token
    )
    {
        return Task.FromResult(new BellTime(entity.Minute, entity.Hour));
    }
}