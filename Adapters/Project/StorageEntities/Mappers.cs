using Domain.Project;
using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal static class Mappers
{
    public static StorageClassroomFeature MapToStorageEntity(this ClassroomFeature entity)
    {
        return new StorageClassroomFeature(entity.Description);
    }

    public static StorageClassroom MapToStorageEntity(this Classroom entity)
    {
        var features = entity.Features.Select(feature =>
            new LinkedEntity<StorageClassroomFeature>(feature.Id));

        return new StorageClassroom(entity.Number, entity.Capacity, features);
    }

    public static StorageDiscipline MapToStorageEntity(this Discipline entity)
    {
        var requirements = entity.ClassroomRequirements.Select(requirement =>
            new LinkedEntity<StorageClassroomFeature>(requirement.Id));

        return new StorageDiscipline(entity.Name, requirements);
    }

    public static StorageLecturer MapToStorageEntity(this Lecturer entity)
    {
        var disciplines = entity.Disciplines.Select(discipline =>
            new LinkedEntity<StorageDiscipline>(discipline.Id));

        return new StorageLecturer(entity.Name, entity.Surname, entity.Patronymic, disciplines);
    }

    public static StorageGroup MapToStorageEntity(this Group entity)
    {
        var disciplines = entity.Disciplines.Select(discipline =>
            new LinkedEntity<StorageDiscipline>(discipline.Id));

        return new StorageGroup(entity.Name, entity.StudentsNumber, disciplines);
    }

    public static StorageExam MapToStorageEntity(this Exam entity)
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