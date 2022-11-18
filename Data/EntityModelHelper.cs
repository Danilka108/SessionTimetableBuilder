using Domain;
using Storage;

namespace Data;

internal abstract class EntityModelHelper<TEntity, TModel>
{
    public abstract TEntity ConvertModelToEntity(TModel model);

    public Identified<TEntity> ConvertModelToEntity(IdentifiedModel<TModel> identifiedModel)
    {
        return new Identified<TEntity>
            (identifiedModel.Id, ConvertModelToEntity(identifiedModel.Model));
    }

    public IEnumerable<LinkedEntity<TEntity>> LinkedEntitiesFromIdentifiedModels
    (
        IEnumerable<IdentifiedModel<TModel>> identifiedModels
    )
    {
        return identifiedModels.Select(LinkedEntityFromIdentifiedModel);
    }

    private static LinkedEntity<TEntity> LinkedEntityFromIdentifiedModel
        (IdentifiedModel<TModel> identifiedModel)
    {
        return new LinkedEntity<TEntity>(identifiedModel.Id);
    }
}