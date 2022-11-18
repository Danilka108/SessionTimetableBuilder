using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class StorageGroupRepository : BaseStorageRepository<GroupEntity, Group>, IGroupRepository
{
    private readonly IBaseRepository<Discipline> _disciplineBaseRepository;

    public StorageGroupRepository
        (StorageProvider storageProvider, IBaseRepository<Discipline> disciplineBaseRepository)
        : base(storageProvider.ProvideStorage(), new GroupEntity.Helper())
    {
        _disciplineBaseRepository = disciplineBaseRepository;
    }

    protected override async Task<Group> ProduceModelByEntity
    (
        GroupEntity groupEntity,
        CancellationToken token
    )
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in groupEntity.ExaminationDisciplines)
        {
            var modelDiscipline = await _disciplineBaseRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Group(groupEntity.Name, groupEntity.StudentsNumber, modelDisciplines);
    }
}