using Storage;

namespace Data.Project.Entities;

internal record Group
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<Discipline>> ExaminationDisciplines
)
{
    public class Helper : EntityModelHelper<Group, Domain.Models.Group>
    {
        public override Group ConvertModelToEntity(Domain.Models.Group model)
        {
            var disciplineConverter = new Discipline.Helper();
            var linkedDisciplines =
                disciplineConverter.LinkedEntitiesFromIdentifiedModels(model.ExaminationDisciplines);

            return new Group(model.Name, model.StudentsNumber, linkedDisciplines);
        }
    }
}