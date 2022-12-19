using Adapter.Project.StorageEntities;
using Storage;

namespace Adapter.Project;

public class ProjectStorageInitializer : IDisposable
{
    private readonly StorageInitializer _initializer;

    public ProjectStorageInitializer(IStorageResource resource)
    {
        _initializer = new StorageInitializer(resource);
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

    public void Dispose()
    {
        _initializer.Dispose();
    }
}