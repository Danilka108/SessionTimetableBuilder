using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage;

/// <summary>
///     Class represents initialization of storage.
/// </summary>
public class StorageInitializer
{
    private readonly Resource _resource;
    private readonly Dictionary<string, SerializableStorageSet> _storageSets;

    /// <summary>
    ///     Creation of storage initializer.
    /// </summary>
    /// <param name="metadata">Storage metadata.</param>
    public StorageInitializer(StorageMetadata metadata)
    {
        _resource = new Resource(metadata.FullPath);
        _storageSets = new Dictionary<string, SerializableStorageSet>();
    }

    /// <summary>
    ///     Add entity to storage.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <returns>The same storage initializer</returns>
    public StorageInitializer AddEntity<TEntity>()
    {
        try
        {
            _storageSets.AddSerializableStorageSet<TEntity>(SerializableStorageSet.CreateEmpty());
        }
        finally
        {
            _resource.Dispose();
        }

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
            await _resource.Serialize(_storageSets, token);
        }
        catch (Exception e)
        {
            throw new StorageInitializationException("Failed to initialize storage", e);
        }
        finally
        {
            _resource.Dispose();
        }
    }
}

public class StorageInitializationException : Exception
{
    internal StorageInitializationException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}