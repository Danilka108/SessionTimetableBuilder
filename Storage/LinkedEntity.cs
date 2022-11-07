namespace Storage;

public class LinkedEntity<TEntity> : IResourceConsumer
{
    private Resource? _resource;

    public LinkedEntity(int id)
    {
        Id = id;
        _resource = null;
    }

    public int Id { get; }

    void IResourceConsumer.ConsumeResource(Resource resource)
    {
        _resource = resource;
    }

    public async Task<TEntity> Deref(CancellationToken token)
    {
        var resource = _resource ?? throw new NullReferenceException(nameof(_resource));

        var scheme = await resource.Deserialize(token);
        var set = scheme.GetSetOf<TEntity>();

        (set as IResourceConsumer).ConsumeResource(resource);

        foreach (var identifiedEntity in set)
        {
            if (identifiedEntity.Id != Id) continue;

            identifiedEntity.Entity.ProvideResourceToFields(resource);
            return identifiedEntity.Entity;
        }

        throw new MissingEntityInStorageSetException("Failed to find linked entity in entities set");
    }
}