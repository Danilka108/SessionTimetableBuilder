using Storage.StorageSet;

namespace Storage.Entity;

/// <summary>
///     Class <c>LinkedEntity</c> represent a reference to the storage Entity.
/// </summary>
/// <typeparam name="TEntity">Type of Entity.</typeparam>
public class LinkedEntity<TEntity> : IResourceConsumer
{
    private StorageResource? _resource;

    public LinkedEntity(int id)
    {
        Id = id;
        _resource = null;
    }

    /// <summary>
    ///     Id of Entity.
    /// </summary>
    public int Id { get; }

    void IResourceConsumer.ConsumeResource(StorageResource storageResource)
    {
        _resource = storageResource;
    }

    /// <summary>
    ///     Asynchronously get Entity contained by link.
    /// </summary>
    /// <param name="token">A token that may be used to cancel the async operation.</param>
    /// <returns>An Entity contained by link.</returns>
    /// <exception cref="DereferenceLinkedEntityException">
    ///     Throw if failed to load storage data or could
    ///     not find Entity by id.
    /// </exception>
    public async Task<TEntity> Dereference(CancellationToken token)
    {
        try
        {
            return await TryDereference(token);
        }
        catch (Exception e)
        {
            throw new DereferenceLinkedEntityException("Failed to dereference linked entity", e);
        }
    }

    private async Task<TEntity> TryDereference(CancellationToken token)
    {
        var resource = _resource ?? throw new NullReferenceException(nameof(_resource));

        var scheme = await resource.Deserialize(token);
        var set = scheme.GetSetOf<TEntity>();

        (set as IResourceConsumer).ConsumeResource(resource);

        foreach (var identifiedEntity in set)
        {
            if (identifiedEntity.Id != Id) continue;

            identifiedEntity.Entity.ProvideResourceToFields(resource);
            return identifiedEntity.Entity;
        }

        throw new MissingEntityInStorageSetException
            ("Failed to find linked entity in entities set");
    }

    public async Task<IdentifiedEntity<TEntity>> DereferenceToIdentified(CancellationToken token)
    {
        var entity = await Dereference(token);
        return new IdentifiedEntity<TEntity>(Id, entity);
    }
}

public class DereferenceLinkedEntityException : Exception
{
    internal DereferenceLinkedEntityException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}