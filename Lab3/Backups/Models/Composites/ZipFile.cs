using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Models.Composites;

public class ZipFile : IZipObject
{
    public ZipFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(fileName);
        }

        Name = fileName;
    }

    public string Name { get; }

    public IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry)
    {
        return new FileRepositoryObject(zipEntry.Open, Name);
    }
}