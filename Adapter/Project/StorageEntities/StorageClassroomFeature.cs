using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageClassroomFeature(string Description)
{
    public class Converter : ConverterToStorageEntity<ClassroomFeature, StorageClassroomFeature>
    {
        public override StorageClassroomFeature ToStorageEntity(ClassroomFeature entity)
        {
            return new StorageClassroomFeature(entity.Description);
        }
    }
}