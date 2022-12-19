using Storage;

namespace Data;

internal abstract class ConverterToStorageEntity<TEntity, TStorageEntity>
{
    public abstract TStorageEntity ToStorageEntity(TEntity entity);

    public Domain.Project.Identified<TStorageEntity> ToStorageEntity(Domain.Project.Identified<TEntity> identifiedEntity)
    {
        return new Domain.Project.Identified<TStorageEntity>
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
//
// internal abstract class EntityToSetConverter<TEntity, TSet>
// {
//     public abstract TSet ConvertToStorageEntity(TEntity entity);
//
//     public Domain.Project.Identified<TSet> ConvertEntityToSet(Domain.Project.Identified<TEntity> identifiedEntity)
//     {
//         return new Domain.Project.Identified<TSet>
//             (identifiedEntity.Id, ConvertEntityToSet(identifiedEntity.Entity));
//     }
//
//     public IEnumerable<ILinkedSet<TSet>> LinkedSetsFromIdentifiedEntities(
//         IEnumerable<Domain.Project.Identified<TEntity>> identifiedEntities
//     )
//     {
//         return identifiedEntities.Select(LinkedSetFromIdentifiedEntity);
//     }
//
//     private ILinkedSet<TSet> LinkedSetFromIdentifiedEntity(Domain.Project.Identified<TEntity> identifiedEntity)
//     {
//         return LinkedSetFactory.Provide<TSet>(identifiedEntity.Id);
//     }
// }