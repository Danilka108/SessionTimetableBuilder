using Storage;

namespace Data.ProjectStorage;

public class ProjectStorageInitializer
{
    private readonly StorageMetadata _metadata;


    public ProjectStorageInitializer(ProjectStorageMetadata metadata)
    {
        _metadata = metadata;
    }

    public async Task Initialize(CancellationToken token)
    {
        await new StorageInitializer(_metadata)
            .AddEntity<Teacher>()
            .AddEntity<Group>()
            .AddEntity<Exam>()
            .AddEntity<Discipline>()
            .AddEntity<Audience>()
            .AddEntity<AudienceSpecificity>()
            .AddEntity<BellTime>().Initialize(token);
    }
}