using Storage;

namespace Data.Project.Entities;

internal record Group
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<Discipline>> ExaminationDisciplines
)
{
    public class Helper : EntityModelHelper<Group, Domain.Project.Models.Group>
    {
        public override Group ConvertModelToEntity(Domain.Project.Models.Group model)
        {
            var disciplineConverter = new Discipline.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels(model.ExaminationDisciplines);

            return new Group(model.Name, model.StudentsNumber, linkedDisciplines);
        }
    }
}