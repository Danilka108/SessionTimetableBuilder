using Domain;

namespace ProjectDomain.Models;

public record Discipline(string Name, IEnumerable<IdentifiedModel<AudienceSpecificity>> AudienceRequirements);