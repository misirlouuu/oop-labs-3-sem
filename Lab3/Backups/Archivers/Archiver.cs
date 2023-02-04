using System.IO.Compression;
using Backups.Composites;
using Backups.Interfaces;
using Backups.Repositories;
using Backups.Storages;
using Backups.Visitors;

namespace Backups.Archivers;

public class Archiver : IArchiver
{
    public ZipStorage Archive(IReadOnlyCollection<IRepositoryObject> repositoryObjects, IRepository storageRepository)
    {
        string name = $"storage {Guid.NewGuid()}";

        using Stream stream = storageRepository.OpenWrite($"{name}.zip");
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

        var visitor = new ZipVisitor(archive);

        repositoryObjects.ToList().ForEach(repositoryObject => repositoryObject.Accept(visitor));

        var zipFolder = new ZipFolder(visitor.ZipObjects.Pop(), name);
        var zipStorage = new ZipStorage(storageRepository, zipFolder);

        return zipStorage;
    }
}