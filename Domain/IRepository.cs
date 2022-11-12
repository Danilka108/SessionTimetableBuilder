using Domain.Models;

namespace Domain;

public interface IRepository<TModel>
{
    Task<IdentifiedModel<TModel>> Create(TModel model, CancellationToken token);

    Task Update(IdentifiedModel<TModel> identifiedModel, CancellationToken token);

    Task Delete(int id, CancellationToken token);

    Task<IdentifiedModel<TModel>> Read(int id, CancellationToken token);

    IObservable<IdentifiedModel<TModel>> Observe(int id);
}