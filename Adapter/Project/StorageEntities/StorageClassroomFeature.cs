using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageClassroomFeature(string Description)
{
    public class Converter : EntityToSetConverter<ClassroomFeature, StorageClassroomFeature>
    {
        public Converter(ILinkedSetFactory linkedSetFactory)
            : base(linkedSetFactory)
        {
        }

        public override StorageClassroomFeature ConvertEntityToSet(ClassroomFeature entity)
        {
            return new StorageClassroomFeature(entity.Description);
        }
    }
}