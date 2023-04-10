using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Models.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public IStorage Run(IReadOnlyCollection<BackupObject> trackingObjects, IRepository storageRepository, IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storageRepository);
        ArgumentNullException.ThrowIfNull(archiver);

        IRepositoryObject[] repositoryObjects = trackingObjects
            .Select(backupObject => backupObject.GetRepositoryObject())
            .ToArray();

        return archiver.Archive(repositoryObjects, storageRepository);
    }
}