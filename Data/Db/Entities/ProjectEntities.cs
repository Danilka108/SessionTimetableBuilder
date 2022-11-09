// using Database;

namespace Data.Db.Models;

public class ProjectDatabase
{
}

public record Teacher(string Name, string Surname, string Patronymic,
    IEnumerable<LinkedTableRow<,>> Disciplines, IEnumerable<LinkedTableRow<,>> Exams);

public record Group(string Name, int StudentsNumber, IEnumerable<LinkedTableRow<,>> Exams);

public record Exam
(
    LinkedTableRow<,> Teacher,
    IEnumerable<LinkedTableRow<,>> Groups,
    LinkedTableRow<,> Discipline,
    LinkedTableRow<,> Auditorium,
    LinkedTableRow<,> ExamTimeBounds
);

public record Discipline(string Name, IEnumerable<LinkedTableRow<,>> Requirements);

public record Auditorium(int Capacity, IEnumerable<LinkedTableRow<,>> Requirements);

public record AuditoriumRequirement(string Description);

public record ClassTimeBounds(int ScheduleIndex, LinkedTableRow<,> StartTimeBound,
    LinkedTableRow<,> EndTimeBound);

public record TimeBound(int Hour, int Minute);