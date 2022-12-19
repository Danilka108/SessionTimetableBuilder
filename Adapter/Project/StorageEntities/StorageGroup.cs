using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageGroup
(
    string Name,
    int StudentsNumber,
    IEnumerable<ILinkedSet<StorageDiscipline>> Disciplines
)
{
    public class Converter : EntityToSetConverter<Group, StorageGroup>
    {
        public Converter(ILinkedSetFactory linkedSetFactory) : base(linkedSetFactory)
        {
        }

        public override StorageGroup ConvertEntityToSet(Group entity)
        {
            var disciplineConverter = new StorageDiscipline.Converter(LinkedSetFactory);
            var disciplines =
                disciplineConverter.LinkedSetsFromIdentifiedEntities(entity.Disciplines);

            return new StorageGroup(entity.Name, entity.StudentsNumber, disciplines);
        }
    }
}