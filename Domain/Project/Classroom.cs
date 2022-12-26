namespace Domain.Project;

public record Classroom
(
    int Id,
    int Number,
    int Capacity,
    IEnumerable<ClassroomFeature> Features
)
{
    public bool ContainsFeature(ClassroomFeature featureToSearch)
    {
        var sameFeature =
            Features.FirstOrDefault(feature => feature.Id == featureToSearch.Id);

        return sameFeature is not null;
    }

    public bool MeetsDisciplineRequirements(Discipline discipline)
    {
        return discipline.ClassroomRequirements.All(ContainsFeature);
    }
    
    public class Comparer : EqualityComparer<Classroom>
    {
        public override bool Equals(Classroom? x, Classroom? y)
        {
            if (x is null && y is null) return false;
            return x is not null && y is not null && x.Id == y.Id;
        }

        public override int GetHashCode(Classroom obj)
        {
            return obj.Id;
        }
    }
}