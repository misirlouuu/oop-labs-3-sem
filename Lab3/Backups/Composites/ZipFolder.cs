using System.Collections.ObjectModel;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Composites;

public class ZipFolder : IZipObject
{
    public ZipFolder(IReadOnlyCollection<IZipObject> children, string name)
    {
        ArgumentNullException.ThrowIfNull(children);
        Children = children;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyCollection<IZipObject> Children { get; }

    public IRepositoryObject GetRepositoryObject(ZipArchiveEntry zipEntry)
    {
        IReadOnlyCollection<IRepositoryObject> Factory()
        {
            var archive = new ZipArchive(zipEntry.Open(), ZipArchiveMode.Read);

            var repositoryObjects = archive.Entries
                .Select(entry => Children
                    .First(zipObject => entry.Name == zipObject.Name)
                    .GetRepositoryObject(entry))
                .ToArray();

            return repositoryObjects;
        }

        return new FolderRepositoryObject(Factory, Path.GetFileNameWithoutExtension(Name));
    }
}