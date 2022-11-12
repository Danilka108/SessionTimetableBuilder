using Data.Project.Entities;

namespace Data.Project.Repositories;

internal class AudienceSpecificityRepository : BaseRepository<AudienceSpecificity, Domain.Models.AudienceSpecificity>
{
    public AudienceSpecificityRepository(ProjectStorageProvider projectStorageProvider) : base(
        projectStorageProvider.ProvideStorage(), new AudienceSpecificity.Helper())
    {
    }

    protected override Task<Domain.Models.AudienceSpecificity> ProduceModelByEntity(
        AudienceSpecificity entity, CancellationToken token)
    {
        return Task.FromResult(
            new Domain.Models.AudienceSpecificity(entity.Description)
        );
    }
}