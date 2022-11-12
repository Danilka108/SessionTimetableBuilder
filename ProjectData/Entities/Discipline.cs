using Data;
using Storage;

namespace ProjectData.Entities;

internal record Discipline
(
    string Name,
    IEnumerable<LinkedEntity<AudienceSpecificity>> AudienceRequirements
)
{
    public class Helper : EntityModelHelper<Discipline, ProjectDomain.Models.Discipline>
    {
        public override Discipline ConvertModelToEntity(ProjectDomain.Models.Discipline model)
        {
            var requirementsConverter = new AudienceSpecificity.Helper();
            var linkedRequirements =
                requirementsConverter.LinkedEntitiesFromIdentifiedModels(model.AudienceRequirements);

            return new Discipline(model.Name, linkedRequirements);
        }
    }
}