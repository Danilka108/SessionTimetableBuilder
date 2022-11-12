using Domain;
using Domain.Models;
using AudienceSpecificity = Domain.Models.AudienceSpecificity;
using Discipline = Data.Project.Entities.Discipline;

namespace Data.Project.Repositories;

internal class DisciplineRepository : BaseRepository<Discipline, Domain.Models.Discipline>
{
    private readonly IRepository<AudienceSpecificity> _requirementsRepository;

    public DisciplineRepository(
        ProjectStorageProvider projectStorageProvider,
        IRepository<AudienceSpecificity> requirementsRepository
    ) : base(projectStorageProvider.ProvideStorage(), new Discipline.Helper())
    {
        _requirementsRepository = requirementsRepository;
    }

    protected override async Task<Domain.Models.Discipline> ProvideModelByEntity(Discipline discipline,
        CancellationToken token)
    {
        var modelRequirements = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedRequirement in discipline.AudienceRequirements)
        {
            var modelRequirement = await _requirementsRepository.Read(linkedRequirement.Id, token);
            modelRequirements.Add(modelRequirement);
        }

        return new Domain.Models.Discipline(discipline.Name, modelRequirements);
    }
}