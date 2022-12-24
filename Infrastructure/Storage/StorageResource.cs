using System.IO;
using Storage;

namespace Infrastructure.Storage;

internal class FileStorageResource : StorageResource
{
    public FileStorageResource(string path) : base(path)
    {
    }

    protected override Stream CreateStream()
    {
        return new FileStream
        (
            Path,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite,
            4096,
            FileOptions.Asynchronous
        );
    }
}