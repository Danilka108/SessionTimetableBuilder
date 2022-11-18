using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class StorageAudienceRepository : BaseStorageRepository<AudienceEntity, Audience>,
    IAudienceRepository
{
    private readonly IBaseRepository<AudienceSpecificity> _specificityBaseRepository;

    public StorageAudienceRepository
    (
        StorageProvider storageProvider,
        IBaseRepository<AudienceSpecificity> specificityBaseRepository
    ) : base(storageProvider.ProvideStorage(), new AudienceEntity.Helper())
    {
        _specificityBaseRepository = specificityBaseRepository;
    }

    protected override async Task<Audience> ProduceModelByEntity
    (
        AudienceEntity audienceEntity,
        CancellationToken token
    )
    {
        var modelSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedSpecificity in audienceEntity.Specificities)
        {
            var modelSpecificity = await _specificityBaseRepository.Read
                (linkedSpecificity.Id, token);
            modelSpecificities.Add(modelSpecificity);
        }

        return new Audience
        (
            audienceEntity.Number,
            audienceEntity.Capacity,
            modelSpecificities
        );
    }
}