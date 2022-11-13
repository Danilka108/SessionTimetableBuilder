using Data.Project.Entities;

namespace Data.Project.Repositories;

internal class AudienceSpecificityRepository
    : BaseRepository<AudienceSpecificity, Domain.Project.Models.AudienceSpecificity>
{
    public AudienceSpecificityRepository(StorageProvider storageProvider) : base(
        storageProvider.ProvideStorage(), new AudienceSpecificity.Helper())
    {
    }

    protected override Task<Domain.Project.Models.AudienceSpecificity> ProduceModelByEntity(
        AudienceSpecificity entity, CancellationToken token)
    {
        return Task.FromResult(
            new Domain.Project.Models.AudienceSpecificity(entity.Description)
        );
    }
}