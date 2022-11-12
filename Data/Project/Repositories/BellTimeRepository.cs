using Data.Project.Entities;

namespace Data.Project.Repositories;

internal class BellTimeRepository : BaseRepository<BellTime, Domain.Models.BellTime>
{
    public BellTimeRepository(ProjectStorageProvider projectStorageProvider) : base(
        projectStorageProvider.ProvideStorage(), new BellTime.Helper())
    {
    }

    protected override Task<Domain.Models.BellTime> ProvideModelByEntity(BellTime entity, CancellationToken token)
    {
        return Task.FromResult(new Domain.Models.BellTime(entity.Minute, entity.Hour));
    }
}