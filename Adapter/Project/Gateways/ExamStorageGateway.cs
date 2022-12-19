using Adapter.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapter.Project.Gateways;

internal class ExamStorageGateway : BaseStorageGateway<Exam, StorageExam>, IExamGateway
{
    private readonly IClassroomGateway _classroomGateway;
    private readonly IDisciplineGateway _disciplineGateway;
    private readonly IGroupGateway _groupGateway;
    private readonly ILecturerGateway _lecturerGeteway;

    public ExamStorageGateway(Storage.Storage storage, IClassroomGateway classroomGateway,
        IDisciplineGateway disciplineGateway, IGroupGateway groupGateway,
        ILecturerGateway lecturerGateway) : base(storage, new StorageExam.Converter())
    {
        _classroomGateway = classroomGateway;
        _disciplineGateway = disciplineGateway;
        _groupGateway = groupGateway;
        _lecturerGeteway = lecturerGateway;
    }

    protected override async Task<Exam> ProduceEntity(StorageExam storageEntity,
        CancellationToken token)
    {
        var classroom = await _classroomGateway.Read(storageEntity.Classroom.Id, token);
        var discipline = await _disciplineGateway.Read(storageEntity.Discipline.Id, token);
        var group = await _groupGateway.Read(storageEntity.Group.Id, token);
        var lecturer = await _lecturerGeteway.Read(storageEntity.Lecturer.Id, token);

        return new Exam(lecturer, group, discipline, classroom, storageEntity.StartTime);
    }
}