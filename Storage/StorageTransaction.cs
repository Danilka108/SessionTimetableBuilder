namespace Storage;

/// <summary>
///     <c>StorageTransaction</c> represents atomic changes undone if they fail to apply.
/// </summary>
public class StorageTransaction : IAsyncDisposable
{
    private readonly Resource _resource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;
    private readonly Stream _stream;
    private readonly CancellationToken _token;

    private StorageTransaction
    (
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

    /// <summary>
    ///     Get storage set of TEntity.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <returns><c>StorageSet</c> of entities</returns>
    /// <exception cref="GetStorageSetException">Throw if could not find storage set.</exception>
    public StorageSet<TEntity> InSetOf<TEntity>()
    {
        try
        {
            return _storageSets.GetSetOf<TEntity>(this);
        }
        catch (Exception e)
        {
            throw new GetStorageSetException
                ($"Could not find storage set of type '{typeof(TEntity)}'", e);
        }
    }

    internal void UpdateSetOf<TEntity>(StorageSet<TEntity> storageSet)
    {
        _storageSets.UpdateSetOf(storageSet);
    }

    /// <summary>
    ///     Save changes in file.
    /// </summary>
    /// <exception cref="CommitTransactionException">Throw if failed to save storage data.</exception>
    public async Task Commit()
    {
        try
        {
            await _resource.Serialize(_stream, _storageSets, _token);
        }
        catch (Exception e)
        {
            throw new CommitTransactionException("Failed to save storage data", e);
        }
    }

    internal static async Task<StorageTransaction> CreateWithResource
    (
        Resource resource,
        CancellationToken token
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

public class GetStorageSetException : Exception
{
    internal GetStorageSetException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}

public class CommitTransactionException : Exception
{
    internal CommitTransactionException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}