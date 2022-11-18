using Data.Project.Entities;
using Storage;

namespace Data.Project;

public class ProjectStorageInitializer
{
    private readonly StorageMetadata _metadata;

    public ProjectStorageInitializer(StorageMetadata metadata)
    {
        _metadata = metadata;
    }

    public async Task Initialize(CancellationToken token)
    {
        using var initializer = new StorageInitializer(_metadata);

        await initializer
            .AddEntity<TeacherEntity>()
            .AddEntity<GroupEntity>()
            .AddEntity<ExamEntity>()
            .AddEntity<DisciplineEntity>()
            .AddEntity<AudienceEntity>()
            .AddEntity<AudienceSpecificityEntity>()
            .AddEntity<BellTimeEntity>()
            .Initialize(token);
    }
}