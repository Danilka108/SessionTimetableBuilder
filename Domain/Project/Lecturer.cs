namespace Domain.Project;

public record Lecturer
(
    int Id,
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<Discipline> Disciplines
)
{
    public string FullName => ProduceFullname(Name, Surname, Patronymic);

    public bool ContainsDiscipline(Discipline disciplineToSearch)
    {
        var sameDiscipline =
            Disciplines.FirstOrDefault(discipline =>
                discipline.Id == disciplineToSearch.Id);

        return sameDiscipline is not null;
    }

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
    
    public class Comparer : EqualityComparer<Lecturer>
    {
        public override bool Equals(Lecturer? x, Lecturer? y)
        {
            if (x is null && y is null) return false;
            return x is not null && y is not null && x.Id == y.Id;
        }

        public override int GetHashCode(Lecturer obj)
        {
            return obj.Id;
        }
    }
}