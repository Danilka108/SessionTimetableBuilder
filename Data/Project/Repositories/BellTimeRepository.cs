using Data.Project.Entities;

namespace Data.Project.Repositories;

internal class BellTimeRepository : BaseRepository<BellTime, Domain.Project.Models.BellTime>
{
    public BellTimeRepository(StorageProvider storageProvider) : base
    (
        storageProvider.ProvideStorage(),
        new BellTime.Helper()
    )
    {
    }

    protected override Task<Domain.Project.Models.BellTime> ProduceModelByEntity
    (
        BellTime entity,
        CancellationToken token
    )
    {
        return Task.FromResult(new Domain.Project.Models.BellTime(entity.Minute, entity.Hour));
    }
}