using Domain;
using Domain.Models;
using Discipline = Domain.Models.Discipline;
using Group = Data.Project.Entities.Group;

namespace Data.Project.Repositories;

internal class GroupRepository : BaseRepository<Group, Domain.Models.Group>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public GroupRepository(ProjectStorageProvider projectStorageProvider, IRepository<Discipline> disciplineRepository)
        : base(projectStorageProvider.ProvideStorage(), new Group.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<Domain.Models.Group> ProduceModelByEntity(Group group, CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in group.ExaminationDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Domain.Models.Group(group.Name, group.StudentsNumber, modelDisciplines);
    }
}