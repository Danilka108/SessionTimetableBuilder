namespace Storage;

public class StorageInitializer
{
    private readonly Resource _resource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;
    private readonly CancellationToken _token;

    public StorageInitializer(string directoryPath, string name, CancellationToken token)
    {
        _resource = new Resource(directoryPath, name);
        _storageSets = new Dictionary<string, SerializableStorageSet>();
        _token = token;
    }

    public StorageInitializer AddEntity<TEntity>()
    {
        _storageSets.AddSerializableStorageSet<TEntity>(SerializableStorageSet.CreateEmpty());
        return this;
    }

    public async Task Initialize()
    {
        await _resource.Serialize(_storageSets, _token);
    }
}