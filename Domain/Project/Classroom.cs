namespace Domain.Project;

public record Classroom
(
    int Id,
    int Number,
    int Capacity,
    IEnumerable<ClassroomFeature> Features
);