using Database;

namespace Data.Db;

internal class JsonFileResourceProvider<TDatabase> : DatabaseResourceProvider<TDatabase> where TDatabase : new()
{
    private const int BufferSize = 4096;

    private readonly string _filePath;

    public JsonFileResourceProvider(string filePath)
    {
        _filePath = filePath;
    }

    public override DatabaseResource<TDatabase> TryProvide(CancellationToken token)
    {
        var fileStream = new FileStream
        (
            _filePath,
            FileMode.Open,
            FileAccess.ReadWrite,
            FileShare.ReadWrite,
            useAsync: true,
            bufferSize: BufferSize
        );

        return new JsonFileResource<TDatabase>(fileStream, token);
    }

    public override bool IsNotInitialized()
    {
        return !File.Exists(_filePath);
    }
}