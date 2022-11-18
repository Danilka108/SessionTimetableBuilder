using Data.Project.Entities;
using Domain;
using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Data.Project.Repositories;

internal class StorageTeacherRepository : BaseStorageRepository<TeacherEntity, Teacher>,
    ITeacherRepository
{
    private readonly IBaseRepository<Discipline> _disciplineBaseRepository;

    public StorageTeacherRepository
    (
        StorageProvider storageProvider,
        IBaseRepository<Discipline> disciplineBaseRepository
    )
        : base(storageProvider.ProvideStorage(), new TeacherEntity.Helper())
    {
        _disciplineBaseRepository = disciplineBaseRepository;
    }

    protected override async Task<Teacher> ProduceModelByEntity
    (
        TeacherEntity teacherEntity,
        CancellationToken token
    )
    {
        var modelDisciplines = new List<IdentifiedModel<Discipline>>();

        foreach (var linkedDiscipline in teacherEntity.AcceptedDisciplines)
        {
            var modelDiscipline = await _disciplineBaseRepository.Read(linkedDiscipline.Id, token);
            modelDisciplines.Add(modelDiscipline);
        }

        return new Teacher
            (teacherEntity.Name, teacherEntity.Surname, teacherEntity.Patronymic, modelDisciplines);
    }
}