using Data;
using Storage;

namespace ProjectData.Entities;

internal record Teacher
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<LinkedEntity<Discipline>> AcceptedDisciplines
)
{
    public class Helper : EntityModelHelper<Teacher, ProjectDomain.Models.Teacher>
    {
        public override Teacher ConvertModelToEntity(ProjectDomain.Models.Teacher model)
        {
            var disciplineConverter = new Discipline.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels(model.AcceptedDisciplines);

            return new Teacher(model.Name, model.Surname, model.Patronymic, linkedDisciplines);
        }
    }
}