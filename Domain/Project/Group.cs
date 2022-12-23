namespace Domain.Project;

public record Group
(
    int Id,
    string Name,
    int StudentsNumber,
    IEnumerable<Discipline> Disciplines
);