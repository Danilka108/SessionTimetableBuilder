using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage;

/// <summary>
///     Implementation of data storage with serialization and deserialization of entities sets.
///     Create and manage your own entities by saving changes in file.
/// </summary>
public class Storage
{
    private static readonly Dictionary<string, Resource> Resources;

    private readonly Resource _resource;

    static Storage()
    {
        Resources = new Dictionary<string, Resource>();
    }

    public Storage(storageMetadata metadata)
    {
        _resource = GetResource(metadata.FullPath);
    }

    private static Resource GetResource(string path)
    {
        Resources.TryGetValue(path, out var resource);

        if (resource is { }) return resource;

        var newResource = new Resource(path);
        Resources.Add(path, newResource);
        return newResource;
    }

    /// <summary>
    ///     Load asynchronously storage set of entities.
    /// </summary>
    /// <param name="token">A token that may be used to cancel the async operation.</param>
    /// <typeparam name="TEntity">Type of loaded entities.</typeparam>
    /// <returns>Enumerable of Identified Entities.</returns>
    /// <exception cref="LoadStorageEntitiesException">Throw if failed to load storage set.</exception>
    public async Task<IEnumerable<Identified<TEntity>>> FromSetOf<TEntity>(CancellationToken token)
    {
        try
        {
            return await TryGetFromSetOf<TEntity>(token);
        }
        catch (Exception e)
        {
            throw new LoadStorageEntitiesException(
                $"Failed to load storage set of entities of type '{typeof(TEntity)}'", e);
        }
    }

    /// <summary>
    ///     Observe set of entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of entities.</typeparam>
    /// <returns>The event is generated when a storage set is updated.</returns>
    /// <exception cref="MissingStorageSetException">Throw if could not find storage set.</exception>
    public IObservable<StorageSet<TEntity>> ObserveSetOf<TEntity>()
    {
        return _resource
            .StorageSets
            .Select
            (storageSets =>
                storageSets.GetSetOf<TEntity>()
            );
    }

    private async Task<IEnumerable<Identified<TEntity>>> TryGetFromSetOf<TEntity>(CancellationToken token)
    {
        var scheme = await _resource.Deserialize(token);
        var set = scheme.GetSetOf<TEntity>();
        (set as IResourceConsumer).ConsumeResource(_resource);

        return set;
    }

    // /// <summary>
    // ///     Start transaction to add changes to storage.
    // ///     Storage transaction represents atomic changes undone if they fail to apply.
    // /// </summary>
    // /// <param name="token">A token that may be used to cancel the async operation.</param>
    // /// <returns>Class <c>StorageTransaction</c></returns>


    /// <summary>
    ///     Start transaction to add changes to storage.
    ///     Storage transaction represents atomic changes undone if they fail to apply .
    /// </summary>
    /// <param name="token"></param>
    /// <returns>Class <c>StorageTransaction</c> representing storage transaction.</returns>
    /// <exception cref="StartStorageTransactionException">Failed to load storage data.</exception>
    public async Task<StorageTransaction> StartTransaction(CancellationToken token)
    {
        try
        {
            return await StorageTransaction.CreateWithResource(_resource, token);
        }
        catch (Exception e)
        {
            throw new StartStorageTransactionException("Failed to load storage data", e);
        }
    }
}

public class LoadStorageEntitiesException : Exception
{
    internal LoadStorageEntitiesException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}

public class StartStorageTransactionException : Exception
{
    internal StartStorageTransactionException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}