using Domain.Project.Models;
using Storage;

namespace Data.Project.Entities;

internal record AudienceEntity
(
    int Number,
    int Capacity,
    IEnumerable<LinkedEntity<AudienceSpecificityEntity>> Specificities
)
{
    public class Helper : EntityModelHelper<AudienceEntity, Audience>
    {
        public override AudienceEntity ConvertModelToEntity(Audience model)
        {
            var specificitiesConverter = new AudienceSpecificityEntity.Helper();

            return new AudienceEntity
            (
                model.Number,
                model.Capacity,
                specificitiesConverter.LinkedEntitiesFromIdentifiedModels(model.Specificities)
            );
        }
    }
}