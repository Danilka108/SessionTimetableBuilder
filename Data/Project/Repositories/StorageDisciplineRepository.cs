using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class
    StorageDisciplineRepository : BaseStorageRepository<DisciplineEntity, Discipline>,
        IDisciplineRepository
{
    private readonly IBaseRepository<AudienceSpecificity> _requirementsBaseRepository;

    public StorageDisciplineRepository
    (
        StorageProvider storageProvider,
        IBaseRepository<AudienceSpecificity> requirementsBaseRepository
    ) : base(storageProvider.ProvideStorage(), new DisciplineEntity.Helper())
    {
        _requirementsBaseRepository = requirementsBaseRepository;
    }

    protected override async Task<Discipline> ProduceModelByEntity
    (
        DisciplineEntity disciplineEntity,
        CancellationToken token
    )
    {
        var modelRequirements = new List<IdentifiedModel<AudienceSpecificity>>();

        foreach (var linkedRequirement in disciplineEntity.AudienceRequirements)
        {
            var modelRequirement = await _requirementsBaseRepository.Read
                (linkedRequirement.Id, token);
            modelRequirements.Add(modelRequirement);
        }

        return new Discipline(disciplineEntity.Name, modelRequirements);
    }
}