// ReSharper disable once RedundantUsingDirective

using System.Reactive.Linq;
using Domain;
using Storage;

namespace Data;

public abstract class BaseRepository<TEntity, TModel> : IRepository<TModel>
{
    protected readonly EntityModelHelper<TEntity, TModel> Helper;
    protected readonly Storage.Storage Storage;

    protected BaseRepository(Storage.Storage storage,
        EntityModelHelper<TEntity, TModel> helper)
    {
        Storage = storage;
        Helper = helper;
    }

    public async Task Delete(int id, CancellationToken token)
    {
        await using var transaction = await Storage.StartTransaction(token);

        transaction
            .InSetOf<TEntity>()
            .Delete(id)
            .Save();

        await transaction.Commit();
    }

    public async Task<IdentifiedModel<TModel>> Create(TModel model, CancellationToken token)
    {
        var entity = Helper.ConvertModelToEntity(model);

        await using var transaction = await Storage.StartTransaction(token);

        transaction
            .InSetOf<TEntity>()
            .Add(entity, out var id)
            .Save();
        await transaction.Commit();

        return new IdentifiedModel<TModel>(id, model);
    }

    public async Task Update(IdentifiedModel<TModel> identifiedModel, CancellationToken token)
    {
        await using var transaction = await Storage.StartTransaction(token);

        transaction
            .InSetOf<TEntity>()
            .Update(Helper.ConvertModelToEntity(identifiedModel))
            .Save();

        await transaction.Commit();
    }

    public async Task<IdentifiedModel<TModel>> Read(int id, CancellationToken token)
    {
        var audiences = await Storage
            .FromSetOf<TEntity>(token);

        var identifiedEntity = audiences.WhereId(id);
        var model = await ProduceModelByEntity(identifiedEntity.Entity, token);

        return new IdentifiedModel<TModel>(identifiedEntity.Id, model);
    }

    public IObservable<IdentifiedModel<TModel>> Observe(int id)
    {
        return Storage
            .ObserveFromSetOf<TEntity>()
            .WhereId(id)
            .SelectMany(async (identifiedEntity, token) =>
            {
                var model = await ProduceModelByEntity(identifiedEntity.Entity, token);
                return new IdentifiedModel<TModel>(identifiedEntity.Id, model);
            });
    }

    protected abstract Task<TModel> ProduceModelByEntity(TEntity entity,
        CancellationToken token);
}