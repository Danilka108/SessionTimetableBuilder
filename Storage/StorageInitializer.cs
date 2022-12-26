using Storage.StorageSet;

namespace Storage;

/// <summary>
///     Class represents initialization of storage.
/// </summary>
public class StorageInitializer : IDisposable
{
    private readonly StorageResource _storageResource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;

    /// <summary>
    ///     Creation of storage initializer.
    /// </summary>
    /// <param name="storageResource">Storage resource.</param>
    public StorageInitializer(StorageResource storageResource)
    {
        _storageResource = storageResource;
        _storageSets = new Dictionary<string, SerializableStorageSet>();
    }

    public void Dispose()
    {
        _storageResource.Dispose();
    }

    /// <summary>
    ///     Add entity to storage.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <returns>The same storage initializer</returns>
    public StorageInitializer AddEntity<TEntity>()
    {
        _storageSets.AddSerializableStorageSet<TEntity>(SerializableStorageSet.CreateEmpty());
        return this;
    }

    /// <summary>
    ///     Asynchronously initialize storage.
    /// </summary>
    /// <param name="token">A token that may be used to cancel the async operation.</param>
    /// <exception cref="StorageInitializationException">Throw if failed to initialize storage.</exception>
    public async Task Initialize(CancellationToken token)
    {
        try
        {
            await _storageResource.Serialize(_storageSets, token);
        }
        catch (Exception e)
        {
            throw new StorageInitializationException("Failed to initialize storage", e);
        }
    }
}

public class StorageInitializationException : Exception
{
    internal StorageInitializationException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}