namespace Database;

public class DatabaseTransaction<TDatabase> : IAsyncDisposable where TDatabase : new()
{
    private readonly TDatabase _database;
    private readonly DatabaseResource<TDatabase> _resource;

    internal DatabaseTransaction(DatabaseResource<TDatabase> resource, TDatabase database)
    {
        _resource = resource;
        _database = database;
    }

    public ValueTask DisposeAsync()
    {
        return _resource.DisposeAsync();
    }

    public Table<TColumns> With<TColumns>(Func<TDatabase, Table<TColumns>> tableProvider)
    {
        return tableProvider(_database);
    }

    public async Task CommitAsync()
    {
        await _resource.Clear();
        await _resource.SerializeAsync(_database);
    }

    private static DatabaseTransaction<TDatabase> CreateEmpty(
        DatabaseResourceProvider<TDatabase> resourceProvider,
        CancellationToken token)
    {
        var database = new TDatabase();

        var resource = resourceProvider.Provide(token);

        return new DatabaseTransaction<TDatabase>(resource, database);
    }

    private static async Task<DatabaseTransaction<TDatabase>> CreateDeserialized(
        DatabaseResourceProvider<TDatabase> resourceProvider,
        CancellationToken token)
    {
        var resource = resourceProvider.Provide(token);
        var database = await resource.DeserializeAsync();

        return new DatabaseTransaction<TDatabase>(resource, database);
    }

    internal static async Task<DatabaseTransaction<TDatabase>> Create(
        DatabaseResourceProvider<TDatabase> resourceProvider, CancellationToken token)
    {
        if (resourceProvider.IsNotInitialized()) return CreateEmpty(resourceProvider, token);
        return await CreateDeserialized(resourceProvider, token);
    }
}