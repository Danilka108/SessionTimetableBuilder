namespace Domain.Project;

public record Discipline(int Id, string Name, IEnumerable<ClassroomFeature> ClassroomRequirements);