using System.IO;
using System.Linq;
using Storage;

namespace Infrastructure.Storage;

internal class FileStorageResource : StorageResource
{
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

    public FileStorageResource(string path) : base(path)
    {
    }
}