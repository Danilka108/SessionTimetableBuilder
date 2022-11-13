using Storage;

namespace Data.Project.Entities;

internal record Audience
(
    int Number,
    int Capacity,
    IEnumerable<LinkedEntity<AudienceSpecificity>> Specificities
)
{
    public class Helper : EntityModelHelper<Audience, Domain.Project.Models.Audience>
    {
        public override Audience ConvertModelToEntity(Domain.Project.Models.Audience model)
        {
            var specificitiesConverter = new AudienceSpecificity.Helper();

            return new Audience(model.Number, model.Capacity,
                specificitiesConverter.LinkedEntitiesFromIdentifiedModels(model.Specificities));
        }
    }
}