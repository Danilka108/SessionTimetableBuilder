namespace Data.Project.Entities;

internal record AudienceSpecificity(string Description)
{
    public class Helper : EntityModelHelper<AudienceSpecificity, Domain.Models.AudienceSpecificity>
    {
        public override AudienceSpecificity ConvertModelToEntity(Domain.Models.AudienceSpecificity model)
        {
            return new AudienceSpecificity(model.Description);
        }
    }
}