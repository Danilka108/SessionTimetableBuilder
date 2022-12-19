using Data;
using Domain.Project;

namespace Adapter.Project.StorageEntities;

internal record StorageLecturer
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<ILinkedSet<DisciplineSet>> Disciplines
)
{
    public class Converter : EntityToSetConverter<Lecturer, StorageLecturer>
    {
        public Converter(ILinkedSetFactory linkedSetFactory) : base(linkedSetFactory)
        {
        }

        public override StorageLecturer ConvertEntityToSet(Lecturer entity)
        {
            var disciplineConverter = new DisciplineSet.Converter(LinkedSetFactory);
            var disciplines =
                disciplineConverter.LinkedSetsFromIdentifiedEntities(entity.Disciplines);

            return new StorageLecturer(entity.Name, entity.Surname, entity.Patronymic, disciplines);
        }
    }
}