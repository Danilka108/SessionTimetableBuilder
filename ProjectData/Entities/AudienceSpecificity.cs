using Data;

namespace ProjectData.Entities;

internal record AudienceSpecificity(string Description)
{
    public class Helper : EntityModelHelper<AudienceSpecificity, ProjectDomain.Models.AudienceSpecificity>
    {
        public override AudienceSpecificity ConvertModelToEntity(ProjectDomain.Models.AudienceSpecificity model)
        {
            return new AudienceSpecificity(model.Description);
        }
    }
}