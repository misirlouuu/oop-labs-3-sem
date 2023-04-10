using Backups.Interfaces;

namespace Backups.Entities;

public class RestorePoint
{
    public RestorePoint(IReadOnlyCollection<BackupObject> trackingObjects, IStorage storage)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(storage);

        DateOfCreation = DateTime.Now;
        TrackingObjects = trackingObjects;
        Storage = storage;
    }

    public IReadOnlyCollection<BackupObject> TrackingObjects { get; }
    public DateTime DateOfCreation { get; }
    public IStorage Storage { get; }
}