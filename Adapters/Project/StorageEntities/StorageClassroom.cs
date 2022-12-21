using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageClassroom
(
    int Number,
    int Capacity,
    IEnumerable<LinkedEntity<StorageClassroomFeature>> Features
)
{
    public class Converter : ConverterToStorageEntity<Classroom, StorageClassroom>
    {
        public override StorageClassroom ToStorageEntity(Classroom entity)
        {
            var featureConverter = new StorageClassroomFeature.Converter();
            var features = featureConverter.ToLinkedEntities(entity.Features);

            return new StorageClassroom(entity.Number, entity.Capacity, features);
        }
    }
}