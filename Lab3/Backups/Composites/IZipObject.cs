using System.IO.Compression;

namespace Backups.Composites;

public interface IZipObject
{
    string Name { get; }
    IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry);
}