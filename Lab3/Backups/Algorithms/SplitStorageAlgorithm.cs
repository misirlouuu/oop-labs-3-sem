using Backups.Archivers;
using Backups.Composites;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public IStorage Run(IReadOnlyCollection<BackupObject> trackingObjects, IRepository storageRepository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storageRepository);
        ArgumentNullException.ThrowIfNull(archiver);

        var zipStorages = trackingObjects
            .Select(backupObject => new List<IRepositoryObject> { backupObject.GetRepositoryObject() })
            .Select(repositoryObjects => archiver.Archive(repositoryObjects, storageRepository))
            .ToArray();

        return new SplitStorage(storageRepository, zipStorages);
    }
}