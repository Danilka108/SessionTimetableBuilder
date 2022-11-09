namespace Storage;

/// <summary>
///     Represents metadata of the storage.
/// </summary>
public class storageMetadata
{
    private const string FileExtension = "json";
    private readonly string _name;

    private readonly string _path;

    /// <summary>
    ///     Create new instance of storage metadata by path and name.
    /// </summary>
    /// <param name="path">Path to directory contains storage file.</param>
    /// <param name="name">Name of storage.</param>
    public storageMetadata(string path, string name)
    {
        _path = path;
        _name = name;
    }

    internal string FullPath => _path + _name + "." + FileExtension;
}