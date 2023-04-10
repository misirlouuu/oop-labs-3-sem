using Backups.Entities;

namespace Backups.Exceptions;

public class RestorePointIsNotFoundException : BackupException
{
    public RestorePointIsNotFoundException(RestorePoint restorePoint)
        : base($"restore point with date of creation {restorePoint.DateOfCreation} is not found in backup copy") { }
}