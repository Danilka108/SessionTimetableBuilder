using Data;
using ProjectData.Entities;

namespace ProjectData.Repositories;

internal class BellTimeRepository : BaseRepository<BellTime, ProjectDomain.Models.BellTime>
{
    public BellTimeRepository(StorageProvider storageProvider) : base(
        storageProvider.ProvideStorage(), new BellTime.Helper())
    {
    }

    protected override Task<ProjectDomain.Models.BellTime> ProduceModelByEntity(BellTime entity,
        CancellationToken token)
    {
        return Task.FromResult(new ProjectDomain.Models.BellTime(entity.Minute, entity.Hour));
    }
}