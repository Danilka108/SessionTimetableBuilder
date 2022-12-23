using Storage.StorageSet;

namespace Storage;

public interface IStorageEditor
{
    /// <summary>
    ///     Get storage set of TEntity.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <returns><c>StorageSet</c> of entities</returns>
    /// <exception cref="GetStorageSetException">Throw if could not find storage set.</exception>
    public StorageSet<TEntity> InSetOf<TEntity>();
}