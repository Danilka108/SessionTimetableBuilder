using Domain.Project.Models;
using Storage;

namespace Data.Project.Entities;

internal record DisciplineEntity
(
    string Name,
    IEnumerable<LinkedEntity<AudienceSpecificityEntity>> AudienceRequirements
)
{
    public class Helper : EntityModelHelper<DisciplineEntity, Discipline>
    {
        public override DisciplineEntity ConvertModelToEntity(Discipline model)
        {
            var requirementsConverter = new AudienceSpecificityEntity.Helper();
            var linkedRequirements =
                requirementsConverter.LinkedEntitiesFromIdentifiedModels
                    (model.AudienceRequirements);

            return new DisciplineEntity(model.Name, linkedRequirements);
        }
    }
}