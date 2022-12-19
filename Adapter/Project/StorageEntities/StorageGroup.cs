using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapter.Project.StorageEntities;

internal record StorageGroup
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<StorageDiscipline>> Disciplines
)
{
    public class Converter : ConverterToStorageEntity<Group, StorageGroup>
    {
        public override StorageGroup ToStorageEntity(Group entity)
        {
            var disciplineConverter = new StorageDiscipline.Converter();
            var disciplines =
                disciplineConverter.ToLinkedEntities(entity.Disciplines);

            return new StorageGroup(entity.Name, entity.StudentsNumber, disciplines);
        }
    }
}