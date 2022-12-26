namespace Domain.Project;

public record ClassroomFeature(int Id, string Description)
{
    public class Comparer : EqualityComparer<ClassroomFeature>
    {
        public override bool Equals(ClassroomFeature? x, ClassroomFeature? y)
        {
            if (x is null && y is null) return false;
            return x is not null && y is not null && x.Id == y.Id;
        }

        public override int GetHashCode(ClassroomFeature obj)
        {
            return obj.Id;
        }
    }
}