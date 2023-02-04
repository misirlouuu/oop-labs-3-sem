using Backups.Entities;

namespace Backups.Exceptions;

public class RestorePointAlreadyExistsException : BackupException
{
    public RestorePointAlreadyExistsException(RestorePoint restorePoint)
        : base($"restore point with date of creation {restorePoint.DateOfCreation} already added in backup copy") { }
}