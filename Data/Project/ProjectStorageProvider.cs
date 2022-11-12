namespace Data.Project;

internal class ProjectStorageProvider : IDisposable
{
    private readonly Storage.Storage _storage;

    public ProjectStorageProvider(ProjectStorageMetadata metadata)
    {
        _storage = new Storage.Storage(metadata);
    }

    public void Dispose()
    {
        _storage.Dispose();
    }

    public Storage.Storage ProvideStorage()
    {
        return _storage;
    }
}