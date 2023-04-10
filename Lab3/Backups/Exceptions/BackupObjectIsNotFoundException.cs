using Backups.Entities;

namespace Backups.Exceptions;

public class BackupObjectIsNotFoundException : BackupException
{
    public BackupObjectIsNotFoundException()
        : base($"backupObject is not found in this directory") { }
}