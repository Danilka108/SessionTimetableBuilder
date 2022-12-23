using Storage.StorageSet;

namespace Storage;

public interface IStorageReader
{
    public Task<IEnumerable<IdentifiedEntity<TEntity>>> FromSetOf<TEntity>(CancellationToken token);
}