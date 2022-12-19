using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class
    DisciplineStorageRepository : BaseStorageRepository<DisciplineSet, Discipline>,
        IDisciplineRepository
{
    private readonly IBaseRepository<AudienceSpecificity> _requirementsBaseRepository;

    public DisciplineStorageRepository
    (
        StorageProvider storageProvider,
        IBaseRepository<AudienceSpecificity> requirementsBaseRepository
    ) : base(storageProvider.ProvideStorage(), new DisciplineSet.Helper())
    {
        _requirementsBaseRepository = requirementsBaseRepository;
    }

    protected override async Task<Discipline> ProduceModelByEntity
    (
        DisciplineSet disciplineSet,
        CancellationToken token
    )
    {
        var modelRequirements = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedRequirement in disciplineSet.AudienceRequirements)
        {
            var modelRequirement = await _requirementsBaseRepository.Read
                (linkedRequirement.Id, token);
            modelRequirements.Add(modelRequirement);
        }

        return new Discipline(disciplineSet.Name, modelRequirements);
    }
}