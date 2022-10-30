namespace App.Ui.Models;

public record Teacher(string Name, string Surname, string Patronymic, Schedule Schedule)
{
    public string NameWithInitials => Name +
                                      (Surname.Length != 0 ? " " + Surname[0] + "." : "") +
                                      (Patronymic.Length != 0 ? " " + Patronymic[0] + "." : "");
}