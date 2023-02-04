namespace Backups.Entities;

public interface IBackup
{
    RestorePoint AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}