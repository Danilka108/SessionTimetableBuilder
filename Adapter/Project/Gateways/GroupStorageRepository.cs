using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Adapter.Project.Gateways;

internal class GroupStorageRepository : BaseStorageRepository<GroupSet, Group>, IGroupRepository
{
    private readonly IBaseRepository<Discipline> _disciplineBaseRepository;

    public GroupStorageRepository
        (StorageProvider storageProvider, IBaseRepository<Discipline> disciplineBaseRepository)
        : base(storageProvider.ProvideStorage(), new GroupSet.Helper())
    {
        _disciplineBaseRepository = disciplineBaseRepository;
    }

    protected override async Task<Group> ProduceModelByEntity
    (
        GroupSet groupSet,
        CancellationToken token
    )
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in groupSet.ExaminationDisciplines)
        {
            var modelDiscipline = await _disciplineBaseRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Group(groupSet.Name, groupSet.StudentsNumber, modelDisciplines);
    }
}