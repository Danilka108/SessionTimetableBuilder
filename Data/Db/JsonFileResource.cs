using System.Text.Json;
using Database;

namespace Data.Db;

internal class JsonFileResource<TDatabase> : DatabaseResource<TDatabase> where TDatabase : new()
{
    public JsonFileResource(Stream stream, CancellationToken token) : base(stream, token)
    {
    }

    protected override async Task TrySerializeAsync(TDatabase database)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        await JsonSerializer.SerializeAsync(Stream, database, options, Token);
    }

    protected override async Task<TDatabase?> TryDeserializeAsync()
    {
        return await JsonSerializer.DeserializeAsync<TDatabase>(Stream, new JsonSerializerOptions(), Token);
    }
}