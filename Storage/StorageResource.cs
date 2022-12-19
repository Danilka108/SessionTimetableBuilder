using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;

namespace Storage;

public abstract class StorageResource : IDisposable
{
    private readonly BehaviorSubject<Dictionary<string, SerializableStorageSet>?>
        _cachedStorageSets;

    private readonly string _path;

    public StorageResource(string path)
    {
        _path = path;
        _cachedStorageSets = new BehaviorSubject<Dictionary<string, SerializableStorageSet>?>(null);

        StorageSets = _cachedStorageSets.AsObservable()
            .SelectMany
            (
                async (storageSets, _, token) =>
                    storageSets ?? await Deserialize(token)
            );
    }

    internal IObservable<Dictionary<string, SerializableStorageSet>> StorageSets { get; }

    private static JsonSerializerOptions JsonOptions =>
        new()
        {
            WriteIndented = true,
            Converters =
            {
                new SerializableStorageSet.ConverterFactory(),
                new SerializableEntity.ConverterFactory()
            }
        };

    public void Dispose()
    {
        _cachedStorageSets.OnCompleted();
        _cachedStorageSets.Dispose();
    }

    // public Stream GetStream()
    // {
    //     try
    //     {
    //         return CreateStream();
    //     }
    //     catch (Exception e)
    //     {
    //         throw new AccessStorageResourceException
    //             ("Failed to create stream of storage resource ", e);
    //     }
    // }

    public abstract Stream GetStream();

    private Stream CreateStream()
    {
        return new FileStream
        (
            _path,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite,
            4096,
            FileOptions.Asynchronous
        );
    }

    internal async Task Serialize
    (
        Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token
    )
    {
        await using var stream = GetStream();
        await Serialize(stream, storageSets, token);
    }

    internal async Task Serialize
    (
        Stream stream,
        Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token
    )
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

    private async Task TrySerialize
    (
        Stream stream,
        Dictionary<string, SerializableStorageSet> storageSets,
        CancellationToken token
    )
    {
        stream.SetLength(0);
        await stream.FlushAsync(token);

        await JsonSerializer.SerializeAsync(stream, storageSets, JsonOptions, token);
        await stream.FlushAsync(token);

        // _cachedStorageSets = storageSets;
        _cachedStorageSets.OnNext(storageSets);
    }

    internal async Task<Dictionary<string, SerializableStorageSet>> Deserialize
        (CancellationToken token)
    {
        await using var stream = GetStream();
        return await Deserialize(stream, token);
    }

    internal async Task<Dictionary<string, SerializableStorageSet>> Deserialize
        (Stream stream, CancellationToken token)
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

    private async Task<Dictionary<string, SerializableStorageSet>?> TryDeserialize
    (
        Stream stream,
        CancellationToken token
    )
    {
        if (_cachedStorageSets.Value is { } cachedStorageSets) return cachedStorageSets;
        // if (_cachedStorageSets is { }) return _cachedStorageSets;

        return await JsonSerializer.DeserializeAsync<Dictionary<string, SerializableStorageSet>>
        (
            stream,
            JsonOptions,
            token
        );
    }
}


public class AccessStorageResourceException : Exception
{
    internal AccessStorageResourceException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}

public class SerializeStorageException : Exception
{
    internal SerializeStorageException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}