using Data.Project.Entities;
using Domain;
using AudienceSpecificity = Domain.Project.Models.AudienceSpecificity;

namespace Data.Project.Repositories;

internal class AudienceRepository : BaseRepository<Audience, Domain.Project.Models.Audience>
{
    private readonly IRepository<AudienceSpecificity> _specificityRepository;

    public AudienceRepository
    (
        StorageProvider storageProvider,
        IRepository<AudienceSpecificity> specificityRepository
    ) : base(storageProvider.ProvideStorage(), new Audience.Helper())
    {
        _specificityRepository = specificityRepository;
    }

    protected override async Task<Domain.Project.Models.Audience> ProduceModelByEntity
    (
        Audience audience,
        CancellationToken token
    )
    {
        var modelSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedSpecificity in audience.Specificities)
        {
            var modelSpecificity = await _specificityRepository.Read(linkedSpecificity.Id, token);
            modelSpecificities.Add(modelSpecificity);
        }

        return new Domain.Project.Models.Audience
        (
            audience.Number,
            audience.Capacity,
            modelSpecificities
        );
    }
}