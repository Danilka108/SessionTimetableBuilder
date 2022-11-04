namespace Database;

public class DatabaseManager<TDatabase> where TDatabase : new()
{
    private readonly DatabaseResourceProvider<TDatabase> _resourceProvider;

    public DatabaseManager(DatabaseResourceProvider<TDatabase> resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public async Task<IEnumerable<Row<TColumns>>> FromAsync<TColumns>(Func<TDatabase, Table<TColumns>> tableProvider,
        CancellationToken token)
    {
        await using var resource = _resourceProvider.Provide(token);
        var database = await resource.DeserializeAsync();

        return tableProvider(database).Rows;
    }

    public async Task<DatabaseTransaction<TDatabase>> StartTransaction(CancellationToken token)
    {
        return await DatabaseTransaction<TDatabase>.Create(_resourceProvider, token);
    }
}