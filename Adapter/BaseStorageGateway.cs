// ReSharper disable once RedundantUsingDirective

using System.Reactive.Linq;
using Application;

namespace Adapter;

internal abstract class BaseStorageGateway<TSet, TEntity> : IBaseGateway<TEntity>
{
    protected readonly EntityModelHelper<TEntity, TModel> Helper;
    protected readonly Storage.Storage Storage;

    protected BaseStorageGateway
    (
        Storage.Storage storage,
        EntityModelHelper<TEntity, TModel> helper
    )
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
        var entities = await Storage
            .FromSetOf<TEntity>(token);

        var identifiedEntity = entities.WhereId(id);
        var model = await ProduceModelByEntity(identifiedEntity.Entity, token);

        return new IdentifiedModel<TModel>(identifiedEntity.Id, model);
    }

    public async Task<IEnumerable<IdentifiedModel<TModel>>> ReadAll(CancellationToken token)
    {
        var identifiedEntities = await Storage.FromSetOf<TEntity>(token);
        var identifiedModels = new List<IdentifiedModel<TModel>>();

        foreach (var identifiedEntity in identifiedEntities)
        {
            var model = await ProduceModelByEntity(identifiedEntity.Entity, token);
            identifiedModels.Add(new IdentifiedModel<TModel>(identifiedEntity.Id, model));
        }

        return identifiedModels;
    }

    public IObservable<IdentifiedModel<TModel>> Observe(int id)
    {
        return Storage
            .ObserveFromSetOf<TEntity>()
            .WhereId(id)
            .SelectMany
            (
                async (identifiedEntity, token) =>
                {
                    var model = await ProduceModelByEntity(identifiedEntity.Entity, token);
                    return new IdentifiedModel<TModel>(identifiedEntity.Id, model);
                }
            );
    }

    public IObservable<IEnumerable<IdentifiedModel<TModel>>> ObserveAll()
    {
        return Storage
            .ObserveFromSetOf<TEntity>()
            .SelectMany
            (
                async (identifiedEntities, token) =>
                {
                    var identifiedModels = new List<IdentifiedModel<TModel>>();

                    foreach (var identifiedEntity in identifiedEntities)
                    {
                        var model = await ProduceModelByEntity(identifiedEntity.Entity, token);
                        identifiedModels.Add
                            (new IdentifiedModel<TModel>(identifiedEntity.Id, model));
                    }

                    return identifiedModels;
                }
            );
    }

    protected abstract Task<TModel> ProduceModelByEntity
    (
        TEntity entity,
        CancellationToken token
    );
}
