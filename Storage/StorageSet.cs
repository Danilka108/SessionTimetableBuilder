using System.Collections;

namespace Storage;

public record Identified<TEntity>(int Id, TEntity Entity);

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

    public StorageSet<TEntity> Update(Identified<TEntity> identifiedEntityToUpdate)
    {
        var entityToUpdateIndex = _entities
            .FindLastIndex(identifiedEntity => identifiedEntity.Id == identifiedEntityToUpdate.Id);

        if (entityToUpdateIndex < 0)
            throw new MissingEntityInStorageSetException("Missing entity to update in storage set");

        _entities.Insert(entityToUpdateIndex, identifiedEntityToUpdate);

        return this;
    }

    public StorageSet<TEntity> Delete(int entityIdToDelete)
    {
        var isNotRemoved = _entities.RemoveAll(identifiedEntity => identifiedEntity.Id == entityIdToDelete) == 0;

        if (isNotRemoved)
            throw new MissingEntityInStorageSetException("Missing entity to delete in storage set");

        return this;
    }

    public StorageSet<TEntity> Add(TEntity newEntity, out int idOfNewEntity)
    {
        LastId += 1;

        var newIdentifiedEntity = new Identified<TEntity>(LastId, newEntity);
        _entities.Add(newIdentifiedEntity);

        idOfNewEntity = newIdentifiedEntity.Id;
        return this;
    }

    public void Save()
    {
        _transaction?.UpdateSetOf(this);
    }
}

public class MissingEntityInStorageSetException : Exception
{
    internal MissingEntityInStorageSetException(string msg) : base(msg)
    {
    }
}