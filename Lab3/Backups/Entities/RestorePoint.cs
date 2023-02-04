using Backups.Interfaces;
using Backups.Storages;

namespace Backups.Entities;

public class RestorePoint
{
    private IReadOnlyCollection<BackupObject> _trackingObjects;

    public RestorePoint(IReadOnlyCollection<BackupObject> trackingObjects, IStorage storage)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        _trackingObjects = trackingObjects;

        DateOfCreation = DateTime.Now;

        ArgumentNullException.ThrowIfNull(storage);
        Storage = storage;
    }

    public DateTime DateOfCreation { get; }
    public IStorage Storage { get; }
}