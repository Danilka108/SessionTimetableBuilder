namespace Domain.Models;

public record ClassTimeBounds(uint Number, BellTime Start, BellTime End);

public record Teacher(string Name, string Surname, string Patronymic, IEnumerable<ClassTimeBounds> Schedule)
{
    public string NameWithInitials => Name +
                                     (Surname.Length != 0 ? " " + Surname[0] + "." : "") +
                                     (Patronymic.Length != 0 ? " " + Patronymic[0] + "." : "");
}