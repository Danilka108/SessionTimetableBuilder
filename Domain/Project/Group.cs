namespace Domain.Project;

public record Group
(
    string Name,
    int StudentsNumber,
    IEnumerable<Identified<Discipline>> Disciplines
);