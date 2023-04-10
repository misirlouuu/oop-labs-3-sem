using Backups.Entities;
using Backups.Interfaces;
using Backups.Models.Storages;

namespace Backups.Models.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public IStorage Run(IReadOnlyCollection<BackupObject> trackingObjects, IRepository storageRepository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storageRepository);
        ArgumentNullException.ThrowIfNull(archiver);

        ZipStorage[] zipStorages = trackingObjects
            .Select(backupObject => new List<IRepositoryObject> { backupObject.GetRepositoryObject() })
            .Select(repositoryObjects => archiver.Archive(repositoryObjects, storageRepository))
            .ToArray();

        return new SplitStorage(storageRepository, zipStorages);
    }
}