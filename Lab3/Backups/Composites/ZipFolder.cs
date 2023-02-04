using System.Collections.ObjectModel;
using System.IO.Compression;

namespace Backups.Composites;

public class ZipFolder : IZipObject
{
    private readonly List<IZipObject> _children;

    public ZipFolder(List<IZipObject> children, string name)
    {
        ArgumentNullException.ThrowIfNull(children);
        _children = children;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyCollection<IZipObject> Children => _children;

    public IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry)
    {
        IReadOnlyCollection<IRepositoryObject> Factory()
        {
            var archive = new ZipArchive(zipEntry.Open(), ZipArchiveMode.Read);

            var repositoryObjects = archive.Entries
                .Select(entry => _children
                    .First(zipObject => entry.Name.Equals(zipObject.Name))
                    .GetRepositoryObject(entry))
                .ToList();

            return repositoryObjects;
        }

        return new FolderRepositoryObject(Factory, Path.GetFileNameWithoutExtension(Name));
    }
}