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
}