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
}