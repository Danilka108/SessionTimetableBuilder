namespace Domain.Project;

public record Group
(
    int Id,
    string Name,
    int StudentsNumber,
    IEnumerable<Discipline> Disciplines
)
{
    public bool ContainsDiscipline(Discipline disciplineToSearch)
    {
        var sameDiscipline =
            Disciplines.FirstOrDefault(discipline =>
                discipline.Id == disciplineToSearch.Id);

        return sameDiscipline is not null;
    }
    
    public class Comparer : EqualityComparer<Group>
    {
        public override bool Equals(Group? x, Group? y)
        {
            if (x is null && y is null) return false;
            return x is not null && y is not null && x.Id == y.Id;
        }

        public override int GetHashCode(Group obj)
        {
            return obj.Id;
        }
    }
}