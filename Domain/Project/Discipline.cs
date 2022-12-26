namespace Domain.Project;

public record Discipline(int Id, string Name, IEnumerable<ClassroomFeature> ClassroomRequirements)
{
    public bool ContainsRequirement(ClassroomFeature requirementToSearch)
    {
        var sameRequirement =
            ClassroomRequirements.FirstOrDefault(requirement =>
                requirement.Id == requirementToSearch.Id);

        return sameRequirement is not null;
    }

    public class Comparer : EqualityComparer<Discipline>
    {
        public override bool Equals(Discipline? x, Discipline? y)
        {
            if (x is null && y is null) return true;
            if (x is null && y is not null) return false;
            if (x is not null && y is null) return false;
            return x.Id == y.Id;
        }

        public override int GetHashCode(Discipline obj)
        {
            return obj.Id;
        }
    }
}