using Data.Project.Entities;
using Domain;
using AudienceSpecificity = Domain.Project.Models.AudienceSpecificity;

namespace Data.Project.Repositories;

internal class DisciplineRepository : BaseRepository<Discipline, Domain.Project.Models.Discipline>
{
    private readonly IRepository<AudienceSpecificity> _requirementsRepository;

    public DisciplineRepository
    (
        StorageProvider storageProvider,
        IRepository<AudienceSpecificity> requirementsRepository
    ) : base(storageProvider.ProvideStorage(), new Discipline.Helper())
    {
        _requirementsRepository = requirementsRepository;
    }

    protected override async Task<Domain.Project.Models.Discipline> ProduceModelByEntity
    (
        Discipline discipline,
        CancellationToken token
    )
    {
        var modelRequirements = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedRequirement in discipline.AudienceRequirements)
        {
            var modelRequirement = await _requirementsRepository.Read(linkedRequirement.Id, token);
            modelRequirements.Add(modelRequirement);
        }

        return new Domain.Project.Models.Discipline(discipline.Name, modelRequirements);
    }
}