using Domain;

namespace ProjectDomain.Models;

public record Teacher(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<IdentifiedModel<Discipline>> AcceptedDisciplines
);