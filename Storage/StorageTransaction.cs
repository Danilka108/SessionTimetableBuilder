namespace Storage;

public class StorageTransaction : IAsyncDisposable
{
    private readonly Resource _resource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;
    private readonly Stream _stream;
    private readonly CancellationToken _token;

    private StorageTransaction(
        Resource resource,
        Dictionary<string, SerializableStorageSet> storageSets,
        Stream stream,
        CancellationToken token
    )
    {
        _resource = resource;
        _storageSets = storageSets;
        _stream = stream;
        _token = token;
    }

    public async ValueTask DisposeAsync()
    {
        await _stream.DisposeAsync();
    }

    public StorageSet<TEntity> InSetOf<TEntity>()
    {
        return _storageSets.GetSetOf<TEntity>(this);
    }

    internal void UpdateSetOf<TEntity>(StorageSet<TEntity> storageSet)
    {
        _storageSets.UpdateSetOf(storageSet);
    }

    public async Task Commit()
    {
        await _resource.Serialize(_stream, _storageSets, _token);
    }

    internal static async Task<StorageTransaction> CreateWithResource(
        Resource resource, CancellationToken token
    )
    {
        var stream = resource.GetStream();

        try
        {
            var storageSets = await resource.Deserialize(stream, token);
            return new StorageTransaction(resource, storageSets, stream, token);
        }
        catch
        {
            await stream.DisposeAsync();
            throw;
        }
    }
}