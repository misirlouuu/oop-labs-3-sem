using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
using Backups.Exceptions;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Services;

public class BackupTask : IBackupTask
{
    private readonly List<BackupObject> _trackingObjects;
    private readonly IStorageAlgorithm _algorithm;
    private readonly IRepository _repository;
    private readonly IArchiver _archiver;
    private IBackup _backup = new Backup();

    public BackupTask(
        string name,
        List<BackupObject> trackingObjects,
        IStorageAlgorithm algorithm,
        IRepository repository,
        IArchiver archiver)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;

        ArgumentNullException.ThrowIfNull(trackingObjects);
        _trackingObjects = trackingObjects;

        ArgumentNullException.ThrowIfNull(algorithm);
        _algorithm = algorithm;

        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;

        ArgumentNullException.ThrowIfNull(archiver);
        _archiver = archiver;
    }

    public string Name { get; }

    public void Run()
    {
        _repository.ChangeRootDirectory($"Backup Task{Name}{Path.DirectorySeparatorChar}Restore Point{Guid.NewGuid()}");

        IStorage storage = _algorithm.Run(_trackingObjects, _repository, _archiver);
        var restorePoint = new RestorePoint(_trackingObjects, storage);

        _backup.AddRestorePoint(restorePoint);
    }

    public BackupObject AddBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        if (_trackingObjects.Contains(backupObject))
            throw new BackupObjectAlreadyExistsException();
        _trackingObjects.Add(backupObject);

        return backupObject;
    }

    public void DeleteBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        if (!_trackingObjects.Contains(backupObject))
            throw new BackupObjectIsNotFoundException();
        _trackingObjects.Remove(backupObject);
    }
}