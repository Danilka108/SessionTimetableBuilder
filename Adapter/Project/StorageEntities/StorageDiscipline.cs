using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapter.Project.StorageEntities;

internal record StorageDiscipline
(
    string Name,
    IEnumerable<LinkedEntity<StorageClassroomFeature>> Requirements
)
{
    public class Converter : ConverterToStorageEntity<Discipline, StorageDiscipline>
    {
        public override StorageDiscipline ToStorageEntity(Discipline entity)
        {
            var requirementsConverter = new StorageClassroomFeature.Converter();
            var requirements = requirementsConverter
                .ToLinkedEntities(entity.Requirements);

            return new StorageDiscipline(entity.Name, requirements);
        }
    }
}