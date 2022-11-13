using Data.Project.Entities;
using Domain;
using Discipline = Domain.Project.Models.Discipline;

namespace Data.Project.Repositories;

internal class GroupRepository : BaseRepository<Group, Domain.Project.Models.Group>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public GroupRepository(StorageProvider storageProvider, IRepository<Discipline> disciplineRepository)
        : base(storageProvider.ProvideStorage(), new Group.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<Domain.Project.Models.Group> ProduceModelByEntity(Group group,
        CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in group.ExaminationDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Domain.Project.Models.Group(group.Name, group.StudentsNumber, modelDisciplines);
    }
}