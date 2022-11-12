using Data;
using Domain;
using ProjectData.Entities;
using Discipline = ProjectDomain.Models.Discipline;

namespace ProjectData.Repositories;

internal class GroupRepository : BaseRepository<Group, ProjectDomain.Models.Group>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public GroupRepository(StorageProvider storageProvider, IRepository<Discipline> disciplineRepository)
        : base(storageProvider.ProvideStorage(), new Group.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<ProjectDomain.Models.Group> ProduceModelByEntity(Group group, CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in group.ExaminationDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new ProjectDomain.Models.Group(group.Name, group.StudentsNumber, modelDisciplines);
    }
}