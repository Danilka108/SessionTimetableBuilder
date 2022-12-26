using Adapters.Project.StorageEntities;
using Storage;

namespace Adapters.Project;

public class ProjectStorageInitializer : IDisposable
{
    private readonly StorageInitializer _initializer;

    public ProjectStorageInitializer(StorageInitializer initializer)
    {
        _initializer = initializer;
    }

    public void Dispose()
    {
        _initializer.Dispose();
    }

    public async Task Initialize(CancellationToken token)
    {
        await _initializer
            .AddEntity<StorageLecturer>()
            .AddEntity<StorageGroup>()
            .AddEntity<StorageExam>()
            .AddEntity<StorageDiscipline>()
            .AddEntity<StorageClassroom>()
            .AddEntity<StorageClassroomFeature>()
            .Initialize(token);
    }
}