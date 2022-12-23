namespace Domain.Project;

public record Exam
(
    int Id,
    Lecturer Lecturer,
    Group Group,
    Discipline Discipline,
    Classroom Classroom,
    DateTime StartTime
);