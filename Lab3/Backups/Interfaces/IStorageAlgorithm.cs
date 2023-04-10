using Backups.Entities;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Interfaces;

public interface IStorageAlgorithm
{
    IStorage Run(IReadOnlyCollection<BackupObject> trackingObjects, IRepository storageRepository, IArchiver archiver);
}