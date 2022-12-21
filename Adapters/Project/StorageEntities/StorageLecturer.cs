using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageLecturer
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<LinkedEntity<StorageDiscipline>> Disciplines
)
{
    public class Converter : ConverterToStorageEntity<Lecturer, StorageLecturer>
    {
        public override StorageLecturer ToStorageEntity(Lecturer entity)
        {
            var disciplineConverter = new StorageDiscipline.Converter();
            var disciplines = disciplineConverter.ToLinkedEntities(entity.Disciplines);

            return new StorageLecturer(entity.Name, entity.Surname, entity.Patronymic, disciplines);
        }
    }
}