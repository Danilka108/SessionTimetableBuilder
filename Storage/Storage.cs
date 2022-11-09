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
/// <remarks>
///     <c>Storage</c> caches all changes of sets.
///     It's recommended to use one instance of <c>Storage</c> while using it.
/// </remarks>
public class Storage : IDisposable
{
    private readonly Resource _resource;

    public Storage(StorageMetadata metadata)
    {
        _resource = new Resource(metadata.FullPath);
    }

    public void Dispose()
    {
        _resource.Dispose();
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
            )
            .Catch<StorageSet<TEntity>, Exception>
            (e => throw new LoadStorageEntitiesException(
                $"Failed to load storage set of entities of type '{typeof(TEntity)}'", e)
            );
    }

    private async Task<IEnumerable<Identified<TEntity>>> TryGetFromSetOf<TEntity>(CancellationToken token)
    {
        var scheme = await _resource.Deserialize(token);
        var set = scheme.GetSetOf<TEntity>();
        (set as IResourceConsumer).ConsumeResource(_resource);

        return set;
    }

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