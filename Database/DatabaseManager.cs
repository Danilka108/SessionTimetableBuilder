namespace Database;

// public interface IDatabaseResourceProvider
// {
//     public void CreateResource(string databaseName);
//
//     public Stream OpenResourceToReadWriteAsync(string databaseName, CancellationToken token);
// }
//
// public interface IDatabaseSerializer<TDatabase> where TDatabase : new()
// {
//     public Task SerializeAsync(Stream stream, TDatabase database, CancellationToken token);
//
//     public Task<TDatabase?> DeserializeAsync(Stream stream, CancellationToken token);
// }

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