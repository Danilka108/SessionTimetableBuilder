using Storage;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapter;

internal abstract class ConverterToStorageEntity<TEntity, TStorageEntity>
{
    public abstract TStorageEntity ToStorageEntity(TEntity entity);

    public IdentifiedEntity<TStorageEntity> ToStorageEntity(Domain.Project.Identified<TEntity> identifiedEntity)
    {
        return new IdentifiedEntity<TStorageEntity>
            (identifiedEntity.Id, ToStorageEntity(identifiedEntity.Entity));
    }

    public IEnumerable<LinkedEntity<TStorageEntity>> ToLinkedEntities(
        IEnumerable<Domain.Project.Identified<TEntity>> identifiedEntities
    )
    {
        return identifiedEntities.Select(ToLinkedEntity);
    }

    public LinkedEntity<TStorageEntity> ToLinkedEntity(Domain.Project.Identified<TEntity> identifiedEntity)
    {
        return new LinkedEntity<TStorageEntity>(identifiedEntity.Id);
    }
}