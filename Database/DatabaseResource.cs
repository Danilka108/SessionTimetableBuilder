namespace Database;

public abstract class DatabaseResourceProvider<TDatabase> where TDatabase : new()
{
    public abstract DatabaseResource<TDatabase> TryProvide(CancellationToken token);

    public DatabaseResource<TDatabase> Provide(CancellationToken token)
    {
        try
        {
            return TryProvide(token);
        }
        catch (Exception e)
        {
            throw new DatabaseResourceException("Fialed to provide resource", e);
        }
    }

    public abstract bool IsNotInitialized();
}

public abstract class DatabaseResource<TDatabase> : IAsyncDisposable where TDatabase : new()
{
    protected readonly Stream Stream;
    protected readonly CancellationToken Token;

    public DatabaseResource(Stream stream, CancellationToken token)
    {
        Stream = stream;
        Token = token;
    }

    public ValueTask DisposeAsync()
    {
        return Stream.DisposeAsync();
    }

    internal async Task Clear()
    {
        try
        {
            Stream.SetLength(0);
            await Stream.FlushAsync(Token);
        }
        catch (Exception e)
        {
            throw new DatabaseResourceException("Failed to clear resource stream", e);
        }
    }

    protected abstract Task TrySerializeAsync(TDatabase database);

    protected abstract Task<TDatabase?> TryDeserializeAsync();

    internal async Task SerializeAsync(TDatabase database)
    {
        try
        {
            await TrySerializeAsync(database);
            await Stream.FlushAsync(Token);
        }
        catch (Exception e)
        {
            throw new DatabaseResourceException("Failed to serialize database", e);
        }
    }

    internal async Task<TDatabase> DeserializeAsync()
    {
        try
        {
            var deserializedDatabase = await TryDeserializeAsync();
            return deserializedDatabase ?? throw new NullReferenceException(nameof(deserializedDatabase));
        }
        catch (Exception e)
        {
            throw new DatabaseResourceException("Failed to deserialize database", e);
        }
    }
}

public class DatabaseResourceException : Exception
{
    public DatabaseResourceException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}