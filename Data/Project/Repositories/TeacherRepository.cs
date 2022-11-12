using Domain;
using Domain.Models;
using Discipline = Domain.Models.Discipline;
using Teacher = Data.Project.Entities.Teacher;

namespace Data.Project.Repositories;

internal class TeacherRepository : BaseRepository<Teacher, Domain.Models.Teacher>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public TeacherRepository(ProjectStorageProvider projectStorageProvider,
        IRepository<Discipline> disciplineRepository)
        : base(projectStorageProvider.ProvideStorage(), new Teacher.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<Domain.Models.Teacher> ProvideModelByEntity(Teacher teacher, CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in teacher.AcceptedDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Domain.Models.Teacher(teacher.Name, teacher.Surname, teacher.Patronymic, modelDisciplines);
    }
}