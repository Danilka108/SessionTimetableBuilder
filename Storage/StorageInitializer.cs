namespace Storage;

/// <summary>
///     Class represents initialization of storage.
/// </summary>
public class StorageInitializer
{
    private readonly Resource _resource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;
    private readonly CancellationToken _token;

    /// <summary>
    ///     Creation of storage initializer.
    /// </summary>
    /// <param name="directoryPath">Path to directory contains storage file.</param>
    /// <param name="name">Name of storage.</param>
    /// <param name="token">A token that may be used to cancel the async operation.</param>
    public StorageInitializer(string directoryPath, string name, CancellationToken token)
    {
        _resource = new Resource(directoryPath, name);
        _storageSets = new Dictionary<string, SerializableStorageSet>();
        _token = token;
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
    /// <exception cref="StorageInitializationException">Throw if failed to initialize storage.</exception>
    public async Task Initialize()
    {
        try
        {
            await _resource.Serialize(_storageSets, _token);
        }
        catch (Exception e)
        {
            throw new StorageInitializationException("Failed to initialize storage", e);
        }
    }
}

public class StorageInitializationException : Exception
{
    internal StorageInitializationException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}