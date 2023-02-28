using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Services;

public class BackupTask : IBackupTask
{
    private readonly List<BackupObject> _trackingObjects;
    private readonly IStorageAlgorithm _algorithm;
    private readonly IRepository _repository;
    private readonly IArchiver _archiver;

    public BackupTask(
        string name,
        List<BackupObject> trackingObjects,
        IStorageAlgorithm algorithm,
        IRepository repository,
        IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(trackingObjects);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(archiver);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(name);
        }

        Name = name;

        _trackingObjects = trackingObjects;
        _algorithm = algorithm;
        _repository = repository;
        _archiver = archiver;
    }

    public string Name { get; }
    public IBackup Backup { get; } = new Backup();

    public void Run()
    {
        char separator = _repository is InMemoryRepository ? '/' : Path.DirectorySeparatorChar;
        _repository.ChangeRootDirectory($"Backup Task{Name}{separator}Restore Point{Guid.NewGuid()}");

        IStorage storage = _algorithm.Run(_trackingObjects, _repository, _archiver);
        var restorePoint = new RestorePoint(_trackingObjects, storage);

        Backup.AddRestorePoint(restorePoint);
    }

    public BackupObject AddBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        if (_trackingObjects.Contains(backupObject))
        {
            throw new BackupObjectAlreadyExistsException();
        }

        _trackingObjects.Add(backupObject);

        return backupObject;
    }

    public void DeleteBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        if (!_trackingObjects.Contains(backupObject))
        {
            throw new BackupObjectIsNotFoundException();
        }

        _trackingObjects.Remove(backupObject);
    }
}