namespace Domain.Project;

public record Exam
(
    Identified<Lecturer> Lecturer,
    Identified<Group> Group,
    Identified<Discipline> Discipline,
    Identified<Classroom> Classroom,
    DateTime StartTime
);