using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageExam
(
    LinkedEntity<StorageLecturer> Lecturer,
    LinkedEntity<StorageGroup> Group,
    LinkedEntity<StorageDiscipline> Discipline,
    LinkedEntity<StorageClassroom> Classroom,
    DateTime StartTime
);