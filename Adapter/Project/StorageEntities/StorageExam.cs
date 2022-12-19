using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageExam
(
    ILinkedSet<StorageLecturer> Lecturer,
    ILinkedSet<StorageGroup> Group,
    ILinkedSet<StorageDiscipline> Discipline,
    ILinkedSet<StorageClassroom> Classroom,
    DateTime StartTime
)
{
    public class Converter : EntityToSetConverter<Exam, StorageExam>
    {
        public Converter(ILinkedSetFactory linkedSetFactory) : base(linkedSetFactory)
        {
        }

        public override StorageExam ConvertEntityToSet(Exam entity)
        {
            return new StorageExam(
                LinkedSetFactory.Provide<StorageLecturer>(entity.Lecturer.Id),
                LinkedSetFactory.Provide<StorageGroup>(entity.Group.Id),
                LinkedSetFactory.Provide<StorageDiscipline>(entity.Discipline.Id),
                LinkedSetFactory.Provide<StorageClassroom>(entity.Classroom.Id),
                entity.StartTime
            );
        }
    }
}