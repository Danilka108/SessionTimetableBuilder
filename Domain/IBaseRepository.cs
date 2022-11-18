namespace Domain;

public interface IBaseRepository<TModel>
{
    Task<IdentifiedModel<TModel>> Create(TModel model, CancellationToken token);

    Task Update(IdentifiedModel<TModel> identifiedModel, CancellationToken token);

    Task Delete(int id, CancellationToken token);

    Task<IdentifiedModel<TModel>> Read(int id, CancellationToken token);

    Task<IEnumerable<IdentifiedModel<TModel>>> ReadAll(CancellationToken token);

    IObservable<IdentifiedModel<TModel>> Observe(int id);

    IObservable<IEnumerable<IdentifiedModel<TModel>>> ObserveAll();
}