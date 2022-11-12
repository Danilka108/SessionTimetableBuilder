namespace Domain.Repositories;

public interface IIdentifiedModel
{
    int Id { get; }
}

public interface ITestModel : IIdentifiedModel
{
    string Name { get; }
}

public interface IRepository<in TModel, TIdentifiedModel> where TIdentifiedModel : TModel, IIdentifiedModel
{
    Task<TIdentifiedModel> Create(TModel model);

    Task Update(TIdentifiedModel identifiedModel);

    Task Delete(int id);

    Task<TIdentifiedModel> Read(int id);

    IObservable<TIdentifiedModel> Observe(int id);
}