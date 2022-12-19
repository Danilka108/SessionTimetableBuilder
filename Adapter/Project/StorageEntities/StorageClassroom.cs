using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageClassroom
(
    int Number,
    int Capacity,
    IEnumerable<ILinkedSet<StorageClassroomFeature>> Features
)
{
    public class Converter : EntityToSetConverter<Classroom, StorageClassroom>
    {
        public Helper(ILinkedSetFactory linkedSetFactory) : base(linkedSetFactory)
        {
        }

        public override StorageClassroom ConvertEntityToSet(Classroom entity)
        {
            var featureConverter = new StorageClassroomFeature.Converter(LinkedSetFactory);
            var features = featureConverter
                .LinkedSetsFromIdentifiedEntities(entity.Features);

            return new StorageClassroom(entity.Number, entity.Capacity, features);
        }
    }
}