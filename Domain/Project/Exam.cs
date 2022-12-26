using System.Globalization;

namespace Domain.Project;

public record Exam
(
    int Id,
    Lecturer Lecturer,
    Group Group,
    Discipline Discipline,
    Classroom Classroom,
    DateTime StartTime
)
{
    public string StartTimeAsString => StartTime.ToString(CultureInfo.InvariantCulture);
    
    public class Comparer : EqualityComparer<Exam>
    {
        public override bool Equals(Exam? x, Exam? y)
        {
            if (x is null && y is null) return false;
            return x is not null && y is not null && x.Id == y.Id;
        }

        public override int GetHashCode(Exam obj)
        {
            return obj.Id;
        }
    }
}