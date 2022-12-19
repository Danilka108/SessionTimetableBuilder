using System.Text;

namespace Domain.Project;

public record Lecturer
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<Identified<Discipline>> Disciplines
)
{
    public static string ProduceFullname(string? name, string? surname, string? patronymic)
    {
        var capitalizedSurname = surname is not null && surname.Length != 0
            ? char.ToUpper(surname[0]) + surname[1..]
            : "";

        var nameInitial = name is not null && name.Length != 0
            ? name[0].ToString().ToUpper() + "."
            : "";

        var patronymicInitial = patronymic is not null && patronymic.Length != 0
            ? patronymic[0].ToString().ToUpper() + "."
            : "";

        return string.Join(" ", capitalizedSurname, nameInitial, patronymicInitial);
    }

    public string FullName => ProduceFullname(Name, Surname, Patronymic);
}