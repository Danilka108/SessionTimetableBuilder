namespace Domain.Project.Models;

public record Discipline
    (string Name, IEnumerable<IdentifiedModel<AudienceSpecificity>> AudienceRequirements);