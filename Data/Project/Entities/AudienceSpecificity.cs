namespace Data.Project.Entities;

public record AudienceSpecificity(string Description)
{
    public Domain.Models.AudienceSpecificity MapToDomainModel(
        int id)
    {
        return new Domain.Models.AudienceSpecificity(id, Description);
    }
}