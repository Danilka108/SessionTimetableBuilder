namespace Domain.Project.Models;

public record Audience(int Number, int Capacity,
    IEnumerable<IdentifiedModel<AudienceSpecificity>> Specificities);