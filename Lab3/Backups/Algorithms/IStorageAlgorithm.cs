using Backups.Archivers;
using Backups.Entities;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public interface IStorageAlgorithm
{
    IStorage Run(
        IReadOnlyCollection<BackupObject> trackingObjects,
        IRepository storageRepository,
        IArchiver archiver);
}