// ReSharper disable once RedundantUsingDirective

using System.Reactive.Linq;
using Application;
using Domain.Project;
using Storage;

namespace Adapters;

internal abstract class BaseStorageGateway<TEntity, TStorageEntity> : IBaseGateway<TEntity>
{
    private readonly ConverterToStorageEntity<TEntity, TStorageEntity> _converter;
    private readonly Storage.Storage _storage;

    protected BaseStorageGateway
    (
        Storage.Storage storage,
        ConverterToStorageEntity<TEntity, TStorageEntity> converter
    )
    {
        _storage = storage;
        _converter = converter;
    }

    public virtual async Task Delete(int id, CancellationToken token)
    {
        await using var transaction = await _storage.StartTransaction(token);

        transaction
            .InSetOf<TStorageEntity>()
            .Delete(id)
            .Save();

        await transaction.Commit();
    }

    public virtual async Task<Identified<TEntity>> Create(TEntity entity, CancellationToken token)
    {
        var storageEntity = _converter.ToStorageEntity(entity);

        await using var transaction = await _storage.StartTransaction(token);

        transaction
            .InSetOf<TStorageEntity>()
            .Add(storageEntity, out var id)
            .Save();
        await transaction.Commit();

        return new Identified<TEntity>(id, entity);
    }

    public virtual async Task Update(Identified<TEntity> identifiedEntity, CancellationToken token)
    {
        await using var transaction = await _storage.StartTransaction(token);

        transaction
            .InSetOf<TStorageEntity>()
            .Update(_converter.ToStorageEntity(identifiedEntity))
            .Save();

        await transaction.Commit();
    }

    public virtual async Task<Identified<TEntity>> Read(int id, CancellationToken token)
    {
        var storageEntities = await _storage
            .FromSetOf<TStorageEntity>(token);

        var identifiedSetItem = storageEntities.WhereId(id);
        var entity = await ProduceEntity(identifiedSetItem.Entity, token);

        return new Identified<TEntity>(identifiedSetItem.Id, entity);
    }

    public virtual async Task<IEnumerable<Identified<TEntity>>> ReadAll(CancellationToken token)
    {
        var identifiedStorageEntities = await _storage.FromSetOf<TStorageEntity>(token);
        var identifiedEntities = new List<Identified<TEntity>>();

        foreach (var identifiedStorageEntity in identifiedStorageEntities)
        {
            var entity = await ProduceEntity(identifiedStorageEntity.Entity, token);
            identifiedEntities.Add(new Identified<TEntity>(identifiedStorageEntity.Id, entity));
        }

        return identifiedEntities;
    }

    public virtual IObservable<Identified<TEntity>> Observe(int id)
    {
        return _storage
            .ObserveFromSetOf<TStorageEntity>()
            .WhereId(id)
            .SelectMany
            (
                async (identifiedStorageEntity, token) =>
                {
                    var entity = await ProduceEntity(identifiedStorageEntity.Entity, token);
                    return new Identified<TEntity>(identifiedStorageEntity.Id, entity);
                }
            );
    }

    public virtual IObservable<IEnumerable<Identified<TEntity>>> ObserveAll()
    {
        return _storage
            .ObserveFromSetOf<TStorageEntity>()
            .SelectMany
            (
                async (identifiedStorageEntities, token) =>
                {
                    var identifiedEntities = new List<Identified<TEntity>>();

                    foreach (var identifiedStorageEntity in identifiedStorageEntities)
                    {
                        var entity =
                            await ProduceEntity(identifiedStorageEntity.Entity, token);
                        identifiedEntities.Add
                            (new Identified<TEntity>(identifiedStorageEntity.Id, entity));
                    }

                    return identifiedEntities;
                }
            );
    }

    protected abstract Task<TEntity> ProduceEntity
    (
        TStorageEntity storageEntity,
        CancellationToken token
    );
}