using System.Reactive.Linq;
using Storage.StorageSet;

namespace Storage;

public static class ObservableExtensions
{
    public static IObservable<IdentifiedEntity<TEntity>> WhereId<TEntity>
    (
        this IObservable<IEnumerable<IdentifiedEntity<TEntity>>> observableIdentifiedEntities,
        int id
    )
    {
        return observableIdentifiedEntities
            .Select
            (
                identifiedEntities => identifiedEntities.WhereId(id)
            );
    }

    public static IdentifiedEntity<TEntity> WhereId<TEntity>
    (
        this IEnumerable<IdentifiedEntity<TEntity>> identifiedEntities,
        int id
    )
    {
        var identifiedEntity = identifiedEntities.FirstOrDefault
            (identifiedEntity => identifiedEntity.Id == id);

        if (identifiedEntity is null)
            throw new MissingEntityInStorageSetException
            (
                $"Failed to find entity with id '{id}' in enumerable of entities of type '{typeof(TEntity)}'"
            );

        return identifiedEntity;
    }
}