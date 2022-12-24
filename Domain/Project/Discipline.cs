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
}