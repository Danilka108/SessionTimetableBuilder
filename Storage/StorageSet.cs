using System;
using System.Collections;
using System.Collections.Generic;

namespace Storage;

/// <summary>
///     Represents entity with storage set id.
/// </summary>
/// <param name="Id">Id of entity.</param>
/// <param name="Entity">Value of entity.</param>
/// <typeparam name="TEntity">Type of entity.</typeparam>
public record Identified<TEntity>(int Id, TEntity Entity);

/// <summary>
///     Set of entities.
/// </summary>
/// <typeparam name="TEntity">Type of entity.</typeparam>
public class StorageSet<TEntity> : IEnumerable<Identified<TEntity>>, IResourceConsumer
{
    private readonly List<Identified<TEntity>> _entities;
    private readonly StorageTransaction? _transaction;

    internal StorageSet(int lastId, IEnumerable<Identified<TEntity>> entities, StorageTransaction? transaction = null)
    {
        LastId = lastId;
        _entities = new List<Identified<TEntity>>(entities);
        _transaction = transaction;
    }

    internal int LastId { get; private set; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Identified<TEntity>> GetEnumerator()
    {
        return _entities.GetEnumerator();
    }

    void IResourceConsumer.ConsumeResource(Resource resource)
    {
        foreach (var identifiedEntity in _entities) identifiedEntity.Entity.ProvideResourceToFields(resource);
    }

    /// <summary>
    ///     Update entity value by id.
    /// </summary>
    /// <param name="identifiedEntityToUpdate">Entity wrapper to <c cref="Identified{TEntity}">Identified</c></param>
    /// <returns>The same storage set.</returns>
    /// <exception cref="MissingEntityInStorageSetException">Throw if missing entity to update in storage set.</exception>
    public StorageSet<TEntity> Update(Identified<TEntity> identifiedEntityToUpdate)
    {
        var entityToUpdateIndex = _entities
            .FindLastIndex(identifiedEntity => identifiedEntity.Id == identifiedEntityToUpdate.Id);

        if (entityToUpdateIndex < 0)
            throw new MissingEntityInStorageSetException("Missing entity to update in storage set");

        _entities.Insert(entityToUpdateIndex, identifiedEntityToUpdate);

        return this;
    }

    /// <summary>
    ///     Delete entity by id.
    /// </summary>
    /// <param name="entityIdToDelete">Entity wrapper to <c cref="Identified{TEntity}">Identified</c></param>
    /// <returns>The same storage set.</returns>
    /// <exception cref="MissingEntityInStorageSetException">Throw if missing entity to update in storage set.</exception>
    public StorageSet<TEntity> Delete(int entityIdToDelete)
    {
        var isNotRemoved = _entities.RemoveAll(identifiedEntity => identifiedEntity.Id == entityIdToDelete) == 0;

        if (isNotRemoved)
            throw new MissingEntityInStorageSetException("Missing entity to delete in storage set");

        return this;
    }

    /// <summary>
    ///     Add entity to storage set.
    /// </summary>
    /// <param name="newEntity">Value of entity.</param>
    /// <param name="idOfNewEntity">Id of added entity.</param>
    /// <returns>The same storage set.</returns>
    public StorageSet<TEntity> Add(TEntity newEntity, out int idOfNewEntity)
    {
        LastId += 1;

        var newIdentifiedEntity = new Identified<TEntity>(LastId, newEntity);
        _entities.Add(newIdentifiedEntity);

        idOfNewEntity = newIdentifiedEntity.Id;
        return this;
    }

    /// <summary>
    ///     Save changes of stage set to transaction.
    /// </summary>
    /// <exception cref="SaveStorageSetException">Throw if failed to save changes to transaction.</exception>
    public void Save()
    {
        try
        {
            _transaction?.UpdateSetOf(this);
        }
        catch (Exception e)
        {
            throw new SaveStorageSetException("Failed to save changes to transaction.", e);
        }
    }
}

public class MissingEntityInStorageSetException : Exception
{
    internal MissingEntityInStorageSetException(string msg) : base(msg)
    {
    }
}

public class SaveStorageSetException : Exception
{
    internal SaveStorageSetException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}