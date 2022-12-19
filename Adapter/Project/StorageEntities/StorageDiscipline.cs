using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageDiscipline
(
    string Name,
    IEnumerable<ILinkedSet<StorageClassroomFeature>> Requirements
)
{
    public class Converter : EntityToSetConverter<Discipline, StorageDiscipline>
    {
        public Helper(ILinkedSetFactory linkedSetFactory) : base(linkedSetFactory)
        {
        }

        public override StorageDiscipline ConvertEntityToSet(Discipline entity)
        {
            var requirementsConverter = new StorageClassroomFeature.Converter(LinkedSetFactory);
            var requirements = requirementsConverter
                .LinkedSetsFromIdentifiedEntities(entity.Requirements);

            return new StorageDiscipline(entity.Name, requirements);
        }
    }
}