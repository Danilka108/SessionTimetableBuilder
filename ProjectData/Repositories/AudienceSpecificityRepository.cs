using Data;
using ProjectData.Entities;

namespace ProjectData.Repositories;

internal class
    AudienceSpecificityRepository : BaseRepository<AudienceSpecificity, ProjectDomain.Models.AudienceSpecificity>
{
    public AudienceSpecificityRepository(StorageProvider storageProvider) : base(
        storageProvider.ProvideStorage(), new AudienceSpecificity.Helper())
    {
    }

    protected override Task<ProjectDomain.Models.AudienceSpecificity> ProduceModelByEntity(
        AudienceSpecificity entity, CancellationToken token)
    {
        return Task.FromResult(
            new ProjectDomain.Models.AudienceSpecificity(entity.Description)
        );
    }
}