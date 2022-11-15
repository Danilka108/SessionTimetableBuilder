namespace Data.Project.Entities;

internal record AudienceSpecificity(string Description)
{
    public class Helper : EntityModelHelper<AudienceSpecificity,
        Domain.Project.Models.AudienceSpecificity>
    {
        public override AudienceSpecificity ConvertModelToEntity
            (Domain.Project.Models.AudienceSpecificity model)
        {
            return new AudienceSpecificity(model.Description);
        }
    }
}