using Domain.Project.Models;

namespace Data.Project.Entities;

internal record AudienceSpecificityEntity(string Description)
{
    public class Helper : EntityModelHelper<AudienceSpecificityEntity,
        AudienceSpecificity>
    {
        public override AudienceSpecificityEntity ConvertModelToEntity
            (AudienceSpecificity model)
        {
            return new AudienceSpecificityEntity(model.Description);
        }
    }
}