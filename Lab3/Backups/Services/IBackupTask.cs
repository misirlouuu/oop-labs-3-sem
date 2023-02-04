using Backups.Entities;

namespace Backups.Services;

public interface IBackupTask
{
    void Run();
    BackupObject AddBackupObject(BackupObject backupObject);
    void DeleteBackupObject(BackupObject backupObject);
}