namespace Domain.Project;

public record Discipline(string Name, IEnumerable<Identified<ClassroomFeature>> Requirements);