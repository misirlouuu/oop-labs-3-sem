using System.IO.Compression;
using Backups.Composites;
using Backups.Repositories;
using Backups.Storages;
using Backups.Visitors;

namespace Backups.Archivers;

public class Archiver : IArchiver
{
    public ZipStorage Archive(IEnumerable<IRepositoryObject> repositoryObjects, IRepository storageRepository)
    {
        string name = $"storage {DateTime.Now}";

        using Stream stream = storageRepository.OpenWrite($"{name}.zip");
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

        var visitor = new ZipVisitor(archive);

        foreach (IRepositoryObject repositoryObject in repositoryObjects)
        {
            repositoryObject.Accept(visitor);
        }

        var zipFolder = new ZipFolder(visitor.ZipObjects.Pop(), name);
        var zipStorage = new ZipStorage(storageRepository, zipFolder);

        return zipStorage;
    }
}