using System.IO.Compression;
using Backups.Composites;

namespace Backups.Interfaces;

public interface IZipObject
{
    string Name { get; }
    IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry);
}