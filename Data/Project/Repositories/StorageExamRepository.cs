using Data.Project.Entities;
using Domain;
using Domain.Project.Models;

namespace Data.Project.Repositories;

internal class StorageExamRepository : BaseStorageRepository<ExamEntity, Exam>
{
    private readonly IBaseRepository<Audience> _audienceBaseRepository;
    private readonly IBaseRepository<BellTime> _bellTimeBaseRepository;
    private readonly IBaseRepository<Discipline> _disciplineBaseRepository;
    private readonly IBaseRepository<Group> _groupBaseRepository;
    private readonly IBaseRepository<Teacher> _teacherBaseRepository;

    public StorageExamRepository
    (
        StorageProvider storageProvider,
        IBaseRepository<Audience> audienceBaseRepository,
        IBaseRepository<Discipline> disciplineBaseRepository,
        IBaseRepository<BellTime> bellTimeBaseRepository,
        IBaseRepository<Group> groupBaseRepository,
        IBaseRepository<Teacher> teacherBaseRepository
    ) : base(storageProvider.ProvideStorage(), new ExamEntity.Helper())
    {
        _audienceBaseRepository = audienceBaseRepository;
        _disciplineBaseRepository = disciplineBaseRepository;
        _bellTimeBaseRepository = bellTimeBaseRepository;
        _groupBaseRepository = groupBaseRepository;
        _teacherBaseRepository = teacherBaseRepository;
    }

    protected override async Task<Exam> ProduceModelByEntity
        (ExamEntity examEntity, CancellationToken token)
    {
        var modelAudience = await _audienceBaseRepository.Read(examEntity.Audience.Id, token);
        var modelDiscipline = await _disciplineBaseRepository.Read(examEntity.Discipline.Id, token);
        var modelGroup = await _groupBaseRepository.Read(examEntity.Group.Id, token);
        var modelTeacher = await _teacherBaseRepository.Read(examEntity.Teacher.Id, token);
        var modelStartBellTime = await _bellTimeBaseRepository.Read
            (examEntity.StartBellTime.Id, token);
        var modelEndBellTime = await _bellTimeBaseRepository.Read(examEntity.EndBellTime.Id, token);

        return new Exam
        (
            modelTeacher,
            modelGroup,
            modelDiscipline,
            modelAudience,
            modelStartBellTime,
            modelEndBellTime,
            examEntity.Day,
            examEntity.Month,
            examEntity.Year
        );
    }
}