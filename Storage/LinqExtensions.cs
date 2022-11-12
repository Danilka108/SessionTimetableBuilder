using System.Reactive.Linq;

namespace Storage;

public static class ObservableExtensions
{
    public static IObservable<Identified<TEntity>> WhereId<TEntity>(
        this IObservable<IEnumerable<Identified<TEntity>>> observableIdentifiedEntities, int id)
    {
        return observableIdentifiedEntities
            .Select(
                identifiedEntities => identifiedEntities.WhereId(id)
            );
    }

    public static Identified<TEntity> WhereId<TEntity>(
        this IEnumerable<Identified<TEntity>> identifiedEntities, int id)
    {
        var identifiedEntity = identifiedEntities.FirstOrDefault(identifiedEntity => identifiedEntity.Id == id);

        if (identifiedEntity is null)
            throw new MissingEntityInStorageSetException(
                $"Failed to find entity with id '{id}' in enumerable of entities of type '{typeof(TEntity)}'");

        return identifiedEntity;
    }
}