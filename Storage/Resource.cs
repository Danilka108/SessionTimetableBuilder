using System.Text.Json;

namespace Storage;

internal class Resource
{
    private const string FileExtension = "json";

    private readonly string _path;

    private Dictionary<string, SerializableStorageSet>? _cachedStorageSets;

    public Resource(string path)
    {
        _path = path;
        _cachedStorageSets = null;
    }

    public Resource(string directoryPath, string name) : this(BuildPath(directoryPath, name))
    {
    }

    private static JsonSerializerOptions JsonOptions => new()
    {
        WriteIndented = true,
        Converters =
        {
            new SerializableStorageSet.ConverterFactory(),
            new SerializableEntity.ConverterFactory()
        }
    };

    public static string BuildPath(string directoryPath, string name)
    {
        return directoryPath + name + "." + FileExtension;
    }

    public Stream GetStream()
    {
        try
        {
            return CreateStream();
        }
        catch (Exception e)
        {
            throw new AccessStorageResourceException("Failed to create stream of storage resource ", e);
        }
    }

    private Stream CreateStream()
    {
        return new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096,
            FileOptions.Asynchronous);
    }

    public async Task Serialize(Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token)
    {
        await using var stream = GetStream();
        await Serialize(stream, storageSets, token);
    }

    public async Task Serialize(Stream stream, Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token)
    {
        try
        {
            await TrySerialize(stream, storageSets, token);
        }
        catch (Exception e)
        {
            throw new SerializeStorageException("Failed to serialize storage data", e);
        }
    }

    private async Task TrySerialize(Stream stream, Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token)
    {
        stream.SetLength(0);
        await stream.FlushAsync(token);

        await JsonSerializer.SerializeAsync(stream, storageSets, JsonOptions, token);
        await stream.FlushAsync(token);

        _cachedStorageSets = storageSets;
    }

    public async Task<Dictionary<string, SerializableStorageSet>> Deserialize(CancellationToken token)
    {
        await using var stream = GetStream();
        return await Deserialize(stream, token);
    }

    public async Task<Dictionary<string, SerializableStorageSet>> Deserialize(Stream stream, CancellationToken token)
    {
        try
        {
            var storageSets = await TryDeserialize(stream, token);
            return storageSets ?? throw new NullReferenceException(nameof(storageSets));
        }
        catch (Exception e)
        {
            throw new SerializeStorageException("Failed to deserialize storage data", e);
        }
    }

    private async Task<Dictionary<string, SerializableStorageSet>?> TryDeserialize(Stream stream,
        CancellationToken token)
    {
        if (_cachedStorageSets is { }) return _cachedStorageSets;

        return await JsonSerializer.DeserializeAsync<Dictionary<string, SerializableStorageSet>>(
            stream, JsonOptions, token
        );
    }
}

internal interface IResourceConsumer
{
    void ConsumeResource(Resource resource);
}

public class AccessStorageResourceException : Exception
{
    internal AccessStorageResourceException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}

public class SerializeStorageException : Exception
{
    internal SerializeStorageException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}