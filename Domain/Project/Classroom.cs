namespace Domain.Project;

public record Classroom
(
    int Number,
    int Capacity,
    IEnumerable<Identified<ClassroomFeature>> Features
);