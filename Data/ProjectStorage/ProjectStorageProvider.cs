using Data.Db.Entities;
using Storage;

namespace Data.Db;

public class ProjectStorageProvider : IDisposable
{
    private readonly Storage.Storage _storage;

    public ProjectStorageProvider(StorageMetadata metadata)
    {
        _storage = new Storage.Storage(metadata);
    }

    public void Dispose()
    {
        _storage.Dispose();
    }

    public async Task InitializeStorage(CancellationToken token)
    {
        await new StorageInitializer(Metadata)
            .AddEntity<Teacher>()
            .AddEntity<Group>()
            .AddEntity<Exam>()
            .AddEntity<Discipline>()
            .AddEntity<Audience>()
            .AddEntity<AudienceSpecificity>()
            .AddEntity<BellTime>().Initialize(token);
    }

    public Storage.Storage ProvideStorage()
    {
        return _storage;
    }
}