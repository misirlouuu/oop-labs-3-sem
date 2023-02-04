using System.IO.Compression;
using Backups.Algorithms;
using Backups.Composites;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Storages;

public class ZipStorage : IStorage
{
    public ZipStorage(IRepository repository, ZipFolder zipFolder)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;

        ArgumentNullException.ThrowIfNull(zipFolder);
        ZipFolder = zipFolder;
    }

    public ZipFolder ZipFolder { get; }
    public IRepository Repository { get; }

    public IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects()
    {
        FileRepositoryObject repositoryObject =
            Repository.GetRepositoryObject(Repository.RootPath) as FileRepositoryObject ??
            throw new ArgumentException();

        using var archive = new ZipArchive(repositoryObject.Stream, ZipArchiveMode.Read);
        ZipArchiveEntry entry = archive.CreateEntry(Repository.RootPath);

        return ZipFolder.Children.Select(zipObject => zipObject.GetRepositoryObject(entry)).ToArray();
    }
}