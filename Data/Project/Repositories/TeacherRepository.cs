using Data.Project.Entities;
using Domain;
using Discipline = Domain.Project.Models.Discipline;

namespace Data.Project.Repositories;

internal class TeacherRepository : BaseRepository<Teacher, Domain.Project.Models.Teacher>
{
    private readonly IRepository<Discipline> _disciplineRepository;

    public TeacherRepository(StorageProvider storageProvider,
        IRepository<Discipline> disciplineRepository)
        : base(storageProvider.ProvideStorage(), new Teacher.Helper())
    {
        _disciplineRepository = disciplineRepository;
    }

    protected override async Task<Domain.Project.Models.Teacher> ProduceModelByEntity(Teacher teacher,
        CancellationToken token)
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in teacher.AcceptedDisciplines)
        {
            var modelDiscipline = await _disciplineRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Domain.Project.Models.Teacher(teacher.Name, teacher.Surname, teacher.Patronymic, modelDisciplines);
    }
}