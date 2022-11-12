using Data;
using Domain;
using ProjectData.Entities;
using AudienceSpecificity = ProjectDomain.Models.AudienceSpecificity;

namespace ProjectData.Repositories;

internal class AudienceRepository : BaseRepository<Audience, ProjectDomain.Models.Audience>
{
    private readonly IRepository<AudienceSpecificity> _specificityRepository;

    public AudienceRepository(
        StorageProvider storageProvider,
        IRepository<AudienceSpecificity> specificityRepository
    ) : base(storageProvider.ProvideStorage(), new Audience.Helper())
    {
        _specificityRepository = specificityRepository;
    }

    protected override async Task<ProjectDomain.Models.Audience> ProduceModelByEntity(
        Audience audience, CancellationToken token)
    {
        var modelSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedSpecificity in audience.Specificities)
        {
            var modelSpecificity = await _specificityRepository.Read(linkedSpecificity.Id, token);
            modelSpecificities.Add(modelSpecificity);
        }

        return new ProjectDomain.Models.Audience(
            audience.Number,
            audience.Capacity,
            modelSpecificities
        );
    }
}