using System.IO.Compression;

namespace Backups.Composites;

public class ZipFile : IZipObject
{
    public ZipFile(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);

        Name = name;
    }

    public string Name { get; }

    public IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry) => new FileRepositoryObject(zipEntry.Open, Name);
}