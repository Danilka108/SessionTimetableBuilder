using Data;
using Storage;

namespace ProjectData.Entities;

internal record Group
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<Discipline>> ExaminationDisciplines
)
{
    public class Helper : EntityModelHelper<Group, ProjectDomain.Models.Group>
    {
        public override Group ConvertModelToEntity(ProjectDomain.Models.Group model)
        {
            var disciplineConverter = new Discipline.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels(model.ExaminationDisciplines);

            return new Group(model.Name, model.StudentsNumber, linkedDisciplines);
        }
    }
}