using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _backupCopy = new ();

    public IReadOnlyCollection<RestorePoint> BackupCopy => _backupCopy;

    public RestorePoint AddRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        if (_backupCopy.Contains(restorePoint))
        {
            throw new RestorePointAlreadyExistsException(restorePoint);
        }

        _backupCopy.Add(restorePoint);

        return restorePoint;
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        if (!_backupCopy.Contains(restorePoint))
        {
            throw new RestorePointIsNotFoundException(restorePoint);
        }

        _backupCopy.Remove(restorePoint);
    }
}