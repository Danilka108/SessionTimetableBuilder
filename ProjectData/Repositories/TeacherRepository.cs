using Data;
using Domain;
using ProjectData.Entities;
using Discipline = ProjectDomain.Models.Discipline;

namespace ProjectData.Repositories;

internal class TeacherRepository : BaseRepository<Teacher, ProjectDomain.Models.Teacher>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public TeacherRepository(StorageProvider storageProvider,
        IRepository<Discipline> disciplineRepository)
        : base(storageProvider.ProvideStorage(), new Teacher.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<ProjectDomain.Models.Teacher> ProduceModelByEntity(Teacher teacher,
        CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in teacher.AcceptedDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new ProjectDomain.Models.Teacher(teacher.Name, teacher.Surname, teacher.Patronymic, modelDisciplines);
    }
}