namespace Domain.Models;

public record Exam
(
    IdentifiedModel<Teacher> Teacher,
    IdentifiedModel<Group> Group,
    IdentifiedModel<Discipline> Discipline,
    IdentifiedModel<Audience> Audience,
    IdentifiedModel<BellTime> StartBellTime,
    IdentifiedModel<BellTime> EndBellTime,
    int Day,
    int Month,
    int Year
);