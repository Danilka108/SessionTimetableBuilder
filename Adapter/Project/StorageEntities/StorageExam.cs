using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapter.Project.StorageEntities;

internal record StorageExam
(
    LinkedEntity<StorageLecturer> Lecturer,
    LinkedEntity<StorageGroup> Group,
    LinkedEntity<StorageDiscipline> Discipline,
    LinkedEntity<StorageClassroom> Classroom,
    DateTime StartTime
)
{
    public class Converter : ConverterToStorageEntity<Exam, StorageExam>
    {
        public override StorageExam ToStorageEntity(Exam entity)
        {
            return new StorageExam(
                new LinkedEntity<StorageLecturer>(entity.Lecturer.Id),
                new LinkedEntity<StorageGroup>(entity.Group.Id),
                new LinkedEntity<StorageDiscipline>(entity.Discipline.Id),
                new LinkedEntity<StorageClassroom>(entity.Classroom.Id),
                entity.StartTime
            );
        }
    }
}