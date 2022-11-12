namespace Storage;

internal static class Extensions
{
    public static void ProvideResourceToFields<TConsumer>(this TConsumer consumer, Resource resource)
    {
        var fields = typeof(TConsumer).GetFields();

        foreach (var field in fields)
            if (field.GetValue(consumer) is IResourceConsumer resourceConsumer)
                resourceConsumer.ConsumeResource(resource);

        var properties = typeof(TConsumer).GetProperties();

        foreach (var property in properties)
            if (property.GetValue(consumer) is IResourceConsumer resourceConsumer)
                resourceConsumer.ConsumeResource(resource);
    }

    public static StorageSet<TEntity> GetSetOf<TEntity>(
        this Dictionary<string, SerializableStorageSet> storageSets,
        StorageTransaction? transaction = null
    )
    {
        var entityTypeName = typeof(TEntity).FullName;
        if (entityTypeName is null) throw new NullReferenceException(entityTypeName);

        foreach (var (name, serializableStorageSet) in storageSets)
        {
            if (name != entityTypeName) continue;
            return serializableStorageSet.ToTypedStorageSet<TEntity>(transaction);
        }

        throw new MissingStorageSetException($"Could not find storage set of type '{typeof(TEntity)}'");
    }

    public static void UpdateSetOf<TEntity>(
        this Dictionary<string, SerializableStorageSet> storageSets,
        StorageSet<TEntity> storageSet)
    {
        try
        {
            var entityFullName = typeof(TEntity).FullName;
            if (entityFullName is null) throw new NullReferenceException(nameof(entityFullName));

            storageSets[entityFullName] = SerializableStorageSet.FromTypedStorageSet(storageSet);
        }
        catch (Exception)
        {
            throw new MissingStorageSetException($"Could not find storage set of type '{typeof(TEntity)}'");
        }
    }

    public static void AddSerializableStorageSet<TEntity>(
        this Dictionary<string, SerializableStorageSet> storageSets,
        SerializableStorageSet storageSet)
    {
        try
        {
            var entityFullName = typeof(TEntity).FullName;

            storageSets.Add(
                entityFullName ?? throw new NullReferenceException(nameof(entityFullName)),
                storageSet
            );
        }
        catch (Exception)
        {
            throw new StorageSetAlreadyExistsExceptions($"Storage set of type '{typeof(TEntity)}' already exists");
        }
    }
}

public class MissingStorageSetException : Exception
{
    internal MissingStorageSetException(string msg) : base(msg)
    {
    }
}

public class StorageSetAlreadyExistsExceptions : Exception
{
    internal StorageSetAlreadyExistsExceptions(string msg) : base(msg)
    {
    }
}