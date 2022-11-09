using Storage;

namespace Data.ProjectStorage.Entities;

public record Exam
(
    LinkedEntity<Teacher> Teacher,
    LinkedEntity<Group> Group,
    LinkedEntity<Discipline> Discipline,
    LinkedEntity<Audience> Audience,
    int Day,
    int Month,
    int Year,
    LinkedEntity<BellTime> StartBellTime,
    LinkedEntity<BellTime> EndBellTime
);

public record BellTime(int Minute, int Hour);

public record Teacher
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<LinkedEntity<Discipline>> AcceptedDisciplines
);

public record Group
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<Discipline>> ExamenationDisciplines
);

public record Discipline
(
    string Name,
    IEnumerable<LinkedEntity<AudienceSpecificity>> AudienceRequirements
);

public record Audience
(
    int Capacity,
    IEnumerable<LinkedEntity<AudienceSpecificity>> AudienceSpecifities
);

public record AudienceSpecificity
(
    string Description
);