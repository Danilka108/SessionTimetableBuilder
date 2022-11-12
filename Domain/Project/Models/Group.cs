namespace Domain.Models;

public record Group(
    string Name,
    int StudentsNumber,
    IEnumerable<IdentifiedModel<Discipline>> ExaminationDisciplines
);