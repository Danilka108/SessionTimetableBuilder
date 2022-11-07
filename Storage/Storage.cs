namespace Storage;

public class Storage
{
    private static readonly Dictionary<string, Resource> Resources;

    private readonly Resource _resource;

    static Storage()
    {
        Resources = new Dictionary<string, Resource>();
    }

    public Storage(string directoryPath, string name)
    {
        var path = Resource.BuildPath(directoryPath, name);
        _resource = GetResource(path);
    }

    private static Resource GetResource(string path)
    {
        Resources.TryGetValue(path, out var resource);

        if (resource is { }) return resource;

        var newResource = new Resource(path);
        Resources.Add(path, newResource);
        return newResource;
    }


    public async Task<IEnumerable<Identified<TEntity>>> FromSetOf<TEntity>(CancellationToken token)
    {
        var scheme = await _resource.Deserialize(token);
        var set = scheme.GetSetOf<TEntity>();
        (set as IResourceConsumer).ConsumeResource(_resource);

        return set;
    }

    public async Task<StorageTransaction> StartTransaction(CancellationToken token)
    {
        return await StorageTransaction.CreateWithResource(_resource, token);
    }
}