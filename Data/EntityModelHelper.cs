using Domain.Models;
using Storage;

namespace Data;

public abstract class EntityModelHelper<TEntity, TModel>
{
    public abstract TEntity ConvertModelToEntity(TModel model);

    public abstract TModel UpdateModelByEntity(TModel updatingModel, TEntity entity);

    public Identified<TEntity> ConvertModelToEntity(IdentifiedModel<TModel> identifiedModel)
    {
        return new Identified<TEntity>(identifiedModel.Id, ConvertModelToEntity(identifiedModel.Model));
    }

    public IdentifiedModel<TModel> UpdateModelByEntity(int id, TModel updatingModel, TEntity entity)
    {
        return new IdentifiedModel<TModel>(id, UpdateModelByEntity(updatingModel, entity));
    }

    public IEnumerable<LinkedEntity<TEntity>> LinkedEntitiesFromIdentifiedModels(
        IEnumerable<IdentifiedModel<TModel>> identifiedModels)
    {
        return identifiedModels.Select(LinkedEntityFromIdentifiedModel);
    }

    public LinkedEntity<TEntity> LinkedEntityFromIdentifiedModel(IdentifiedModel<TModel> identifiedModel)
    {
        return new LinkedEntity<TEntity>(identifiedModel.Id);
    }
}