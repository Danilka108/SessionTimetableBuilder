using System.Reflection;

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
        var attr = typeof(TEntity).GetCustomAttribute<EntityAttribute>() ??
                   throw new MissingEntityAttributeException($"Missing EntityAttribute on {typeof(TEntity)} type");

        foreach (var (name, serializableStorageSet) in storageSets)
        {
            if (name != attr.Name) continue;
            return serializableStorageSet.ToTypedStorageSet<TEntity>(transaction);
        }

        throw new MissingStorageSetException($"Could not find storage set '{attr.Name}'");
    }

    public static void UpdateSetOf<TEntity>(
        this Dictionary<string, SerializableStorageSet> storageSets,
        StorageSet<TEntity> storageSet)
    {
        var attr = typeof(TEntity).GetCustomAttribute<EntityAttribute>() ??
                   throw new MissingEntityAttributeException($"Missing EntityAttribute on {typeof(TEntity)} type");

        try
        {
            storageSets[attr.Name] = SerializableStorageSet.FromTypedStorageSet(storageSet);
        }
        catch (Exception)
        {
            throw new MissingStorageSetException($"Could not find storage set '{attr.Name}'");
        }
    }

    public static void AddSerializableStorageSet<TEntity>(
        this Dictionary<string, SerializableStorageSet> storageSets,
        SerializableStorageSet storageSet)
    {
        var attr = typeof(TEntity).GetCustomAttribute<EntityAttribute>() ??
                   throw new MissingEntityAttributeException($"Missing EntityAttribute on {typeof(TEntity)} type");

        try
        {
            storageSets.Add(attr.Name, storageSet);
        }
        catch (Exception)
        {
            throw new StorageSetAlreadyExistsExceptions($"Storage set '{attr.Name}' already exists");
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