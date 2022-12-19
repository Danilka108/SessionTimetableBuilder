using Domain.Project;

namespace Application;

public interface IBaseGateWay<TEntity>
{
    Task<Identified<TEntity>> Create(TEntity entity, CancellationToken token);

    Task Update(Identified<TEntity> identifiedModel, CancellationToken token);

    Task Delete(int id, CancellationToken token);

    Task<Identified<TEntity>> Read(int id, CancellationToken token);

    Task<IEnumerable<Identified<TEntity>>> ReadAll(CancellationToken token);

    IObservable<Identified<TEntity>> Observe(int id);

    IObservable<IEnumerable<Identified<TEntity>>> ObserveAll();
}