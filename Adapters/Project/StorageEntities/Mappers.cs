using Domain.Project;

namespace Adapters.Project.StorageEntities;

internal static class Mappers
{
    public static StorageClassroomFeature MapToStorageEntity(this ClassroomFeature entity)
    {
        return new StorageClassroomFeature(entity.Description);
    }
}