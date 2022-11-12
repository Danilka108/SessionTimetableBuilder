using Data;
using Storage;

namespace ProjectData.Entities;

internal record Audience
(
    int Number,
    int Capacity,
    IEnumerable<LinkedEntity<AudienceSpecificity>> Specificities
)
{
    public class Helper : EntityModelHelper<Audience, ProjectDomain.Models.Audience>
    {
        public override Audience ConvertModelToEntity(ProjectDomain.Models.Audience model)
        {
            var specificitiesConverter = new AudienceSpecificity.Helper();

            return new Audience(model.Number, model.Capacity,
                specificitiesConverter.LinkedEntitiesFromIdentifiedModels(model.Specificities));
        }
    }
}