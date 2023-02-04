using Backups.Archivers;
using Backups.Composites;
using Backups.Entities;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public IStorage Run(
        IReadOnlyCollection<BackupObject> trackingObjects,
        IRepository storageRepository,
        IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storageRepository);
        ArgumentNullException.ThrowIfNull(archiver);

        var zipStorages = trackingObjects
            .Select(backupObject => new List<IRepositoryObject> { backupObject.GetRepositoryObject() })
            .Select(repositoryObjects => archiver.Archive(repositoryObjects, storageRepository))
            .ToList();

        return new SplitStorage(storageRepository, zipStorages);
    }
}