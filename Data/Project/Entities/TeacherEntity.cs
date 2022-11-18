using Domain.Project.Models;
using Storage;

namespace Data.Project.Entities;

internal record TeacherEntity
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<LinkedEntity<DisciplineEntity>> AcceptedDisciplines
)
{
    public class Helper : EntityModelHelper<TeacherEntity, Teacher>
    {
        public override TeacherEntity ConvertModelToEntity(Teacher model)
        {
            var disciplineConverter = new DisciplineEntity.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels(model.AcceptedDisciplines);

            return new TeacherEntity
                (model.Name, model.Surname, model.Patronymic, linkedDisciplines);
        }
    }
}