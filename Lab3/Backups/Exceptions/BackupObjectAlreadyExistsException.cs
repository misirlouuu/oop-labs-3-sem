namespace Backups.Exceptions;

public class BackupObjectAlreadyExistsException : BackupException
{
    public BackupObjectAlreadyExistsException()
        : base($"backupObject already exists") { }
}