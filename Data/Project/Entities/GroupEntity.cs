using Domain.Project.Models;
using Storage;

namespace Data.Project.Entities;

internal record GroupEntity
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<DisciplineEntity>> ExaminationDisciplines
)
{
    public class Helper : EntityModelHelper<GroupEntity, Group>
    {
        public override GroupEntity ConvertModelToEntity(Group model)
        {
            var disciplineConverter = new DisciplineEntity.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels
                    (model.ExaminationDisciplines);

            return new GroupEntity(model.Name, model.StudentsNumber, linkedDisciplines);
        }
    }
}