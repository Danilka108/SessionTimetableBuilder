using Data.Project.Entities;
using Domain;
using Audience = Domain.Project.Models.Audience;
using BellTime = Domain.Project.Models.BellTime;
using Discipline = Domain.Project.Models.Discipline;
using Group = Domain.Project.Models.Group;
using Teacher = Domain.Project.Models.Teacher;

namespace Data.Project.Repositories;

internal class ExamRepository : BaseRepository<Exam, Domain.Project.Models.Exam>
{
    private readonly IRepository<Audience> _audienceRepository;
    private readonly IRepository<BellTime> _bellTimeRepository;
    private readonly IRepository<Discipline> _disciplineRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IRepository<Teacher> _teacherRepository;

    public ExamRepository
    (
        StorageProvider storageProvider,
        IRepository<Audience> audienceRepository,
        IRepository<Discipline> disciplineRepository,
        IRepository<BellTime> bellTimeRepository,
        IRepository<Group> groupRepository,
        IRepository<Teacher> teacherRepository
    ) : base(storageProvider.ProvideStorage(), new Exam.Helper())
    {
        _audienceRepository = audienceRepository;
        _disciplineRepository = disciplineRepository;
        _bellTimeRepository = bellTimeRepository;
        _groupRepository = groupRepository;
        _teacherRepository = teacherRepository;
    }

    protected override async Task<Domain.Project.Models.Exam> ProduceModelByEntity
        (Exam exam, CancellationToken token)
    {
        var modelAudience = await _audienceRepository.Read(exam.Audience.Id, token);
        var modelDiscipline = await _disciplineRepository.Read(exam.Discipline.Id, token);
        var modelGroup = await _groupRepository.Read(exam.Group.Id, token);
        var modelTeacher = await _teacherRepository.Read(exam.Teacher.Id, token);
        var modelStartBellTime = await _bellTimeRepository.Read(exam.StartBellTime.Id, token);
        var modelEndBellTime = await _bellTimeRepository.Read(exam.EndBellTime.Id, token);

        return new Domain.Project.Models.Exam
        (
            modelTeacher,
            modelGroup,
            modelDiscipline,
            modelAudience,
            modelStartBellTime,
            modelEndBellTime,
            exam.Day,
            exam.Month,
            exam.Year
        );
    }
}