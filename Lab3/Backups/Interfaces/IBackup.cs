using Backups.Entities;

namespace Backups.Interfaces;

public interface IBackup
{
    IReadOnlyCollection<RestorePoint> BackupCopy { get; }
    RestorePoint AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}