using Storage;

namespace Data.Project.Entities;

internal record Discipline
(
    string Name,
    IEnumerable<LinkedEntity<AudienceSpecificity>> AudienceRequirements
)
{
    public class Helper : EntityModelHelper<Discipline, Domain.Project.Models.Discipline>
    {
        public override Discipline ConvertModelToEntity(Domain.Project.Models.Discipline model)
        {
            var requirementsConverter = new AudienceSpecificity.Helper();
            var linkedRequirements =
                requirementsConverter.LinkedEntitiesFromIdentifiedModels
                    (model.AudienceRequirements);

            return new Discipline(model.Name, linkedRequirements);
        }
    }
}