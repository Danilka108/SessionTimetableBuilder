using Data;
using Domain;
using ProjectData.Entities;
using AudienceSpecificity = ProjectDomain.Models.AudienceSpecificity;

namespace ProjectData.Repositories;

internal class DisciplineRepository : BaseRepository<Discipline, ProjectDomain.Models.Discipline>
{
    private readonly IRepository<AudienceSpecificity> _requirementsRepository;

    public DisciplineRepository(
        StorageProvider storageProvider,
        IRepository<AudienceSpecificity> requirementsRepository
    ) : base(storageProvider.ProvideStorage(), new Discipline.Helper())
    {
        _requirementsRepository = requirementsRepository;
    }

    protected override async Task<ProjectDomain.Models.Discipline> ProduceModelByEntity(Discipline discipline,
        CancellationToken token)
    {
        var modelRequirements = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedRequirement in discipline.AudienceRequirements)
        {
            var modelRequirement = await _requirementsRepository.Read(linkedRequirement.Id, token);
            modelRequirements.Add(modelRequirement);
        }

        return new ProjectDomain.Models.Discipline(discipline.Name, modelRequirements);
    }
}