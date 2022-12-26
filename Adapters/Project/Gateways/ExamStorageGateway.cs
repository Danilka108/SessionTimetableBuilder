using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

public class ExamStorageGateway : IExamGateway
{
    private readonly Storage.Storage _storage;

    private readonly DisciplineStorageGateway _disciplineGateway;

    private readonly LecturerStorageGateway _lecturerGateway;

    private readonly GroupStorageGateway _groupGateway;

    private readonly ClassroomStorageGateway _classroomGateway;

    public ExamStorageGateway(
        Storage.Storage storage,
        DisciplineStorageGateway disciplineGateway,
        LecturerStorageGateway lecturerGateway,
        GroupStorageGateway groupGateway,
        ClassroomStorageGateway classroomGateway
    )
    {
        _storage = storage;
        _disciplineGateway = disciplineGateway;
        _lecturerGateway = lecturerGateway;
        _groupGateway = groupGateway;
        _classroomGateway = classroomGateway;
    }

    public async Task<Exam> Create(Lecturer lecturer, Group group, Discipline discipline,
        Classroom classroom,
        DateTime startTime, CancellationToken token)
    {
        var storageExam = new StorageExam(
            new LinkedEntity<StorageLecturer>(lecturer.Id),
            new LinkedEntity<StorageGroup>(group.Id),
            new LinkedEntity<StorageDiscipline>(discipline.Id),
            new LinkedEntity<StorageClassroom>(classroom.Id),
            startTime
        );

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageExam>()
                .Add(storageExam, out var id)
                .Save();

            await t.Commit();

            return new Exam(id, lecturer, group, discipline, classroom, startTime);
        }
        catch (Exception e)
        {
            throw new ExamGatewayException("Failed to create exam", e);
        }
    }

    public async Task Update(Exam exam, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);

            t
                .InSetOf<StorageExam>()
                .Update(new IdentifiedEntity<StorageExam>(exam.Id, exam.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ExamGatewayException("Failed to update exam", e);
        }
    }

    public async Task Delete(Exam exam, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageExam>()
                .Delete(exam.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ExamGatewayException("Failed to delete exam", e);
        }
    }

    public async Task<Exam> Read(int id, CancellationToken token)
    {
        foreach (var exam in await ReadAll(token))
        {
            if (exam.Id != id) continue;

            return exam;
        }

        throw new ExamGatewayException("Could not be found exam");
    }

    public async Task<IEnumerable<Exam>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageExam>> storageExams;

        try
        {
            storageExams = await _storage.FromSetOf<StorageExam>(token);
        }
        catch (Exception e)
        {
            throw new ExamGatewayException("Failed to read exams", e);
        }

        var exams = storageExams
            .Select(storageExam => MapStorageEntityToEntity(storageExam, token));

        return await Task.WhenAll(exams);
    }

    public IObservable<Exam> Observe(int id)
    {
        return ObserveAll()
            .Select(exams =>
            {
                foreach (var exam in exams)
                {
                    if (exam.Id != id) continue;
                    return exam;
                }

                throw new ExamGatewayException("Could not be found exam");
            });
    }

    public IObservable<IEnumerable<Exam>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageExam>()
            .SelectMany(async (storageExams, token) =>
            {
                var exams = new List<Exam>();

                foreach (var storageExam in storageExams)
                {
                    var exam = await MapStorageEntityToEntity(storageExam, token);
                    exams.Add(exam);
                }

                return exams;
            })
            .Catch<IEnumerable<Exam>, Exception>(e =>
                throw new GroupGatewayException("Failed to observe all exams", e)
            );
    }

    private async Task<Exam> MapStorageEntityToEntity
    (
        IdentifiedEntity<StorageExam> storageExam,
        CancellationToken token
    )
    {
        var lecturer = await _lecturerGateway.Read(storageExam.Entity.Lecturer.Id, token);
        var group = await _groupGateway.Read(storageExam.Entity.Group.Id, token);
        var discipline =
            await _disciplineGateway.Read(storageExam.Entity.Discipline.Id, token);
        var classroom =
            await _classroomGateway.Read(storageExam.Entity.Classroom.Id, token);

        return new Exam(storageExam.Id, lecturer, group, discipline, classroom,
            storageExam.Entity.StartTime);
    }
}