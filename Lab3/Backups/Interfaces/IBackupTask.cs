using Backups.Entities;

namespace Backups.Interfaces;

public interface IBackupTask
{
    void Run();
    BackupObject AddBackupObject(BackupObject backupObject);
    void DeleteBackupObject(BackupObject backupObject);
}