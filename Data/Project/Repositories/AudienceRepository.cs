using Domain;
using Domain.Models;
using Audience = Data.Project.Entities.Audience;
using AudienceSpecificity = Domain.Models.AudienceSpecificity;

namespace Data.Project.Repositories;

internal class AudienceRepository : BaseRepository<Audience, Domain.Models.Audience>
{
    private readonly IRepository<AudienceSpecificity> _specificityRepository;

    public AudienceRepository(
        ProjectStorageProvider projectStorageProvider,
        IRepository<AudienceSpecificity> specificityRepository
    ) : base(projectStorageProvider.ProvideStorage(), new Audience.Helper())
    {
        _specificityRepository = specificityRepository;
    }

    protected override async Task<Domain.Models.Audience> ProduceModelByEntity(
        Audience audience, CancellationToken token)
    {
        var modelSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedSpecificity in audience.Specificities)
        {
            var modelSpecificity = await _specificityRepository.Read(linkedSpecificity.Id, token);
            modelSpecificities.Add(modelSpecificity);
        }

        return new Domain.Models.Audience(
            audience.Number,
            audience.Capacity,
            modelSpecificities
        );
    }
}