using Backups.Archivers;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public IStorage Run(IReadOnlyCollection<BackupObject> trackingObjects, IRepository storageRepository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storageRepository);
        ArgumentNullException.ThrowIfNull(archiver);

        var repositoryObjects = trackingObjects
            .Select(backupObject => backupObject.GetRepositoryObject())
            .ToArray();

        return archiver.Archive(repositoryObjects, storageRepository);
    }
}