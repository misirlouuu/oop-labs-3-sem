using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
using Backups.Repositories;
using Backups.Services;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void BackupTaskExecuted_RestorePointAddedToBackup()
    {
        var backupObjectRep = new InMemoryRepository("/backupObjectRep");
        var storageRep = new InMemoryRepository("/storageRep");

        backupObjectRep.WriteAllText(@"/test1.txt", "test file n1");
        backupObjectRep.WriteAllText(@"/test2.txt", "test file n2");

        var algorithm = new SplitStorageAlgorithm();
        var archiver = new Archiver();

        var backupObject1 = new BackupObject(backupObjectRep, @"/test1.txt");
        var backupObject2 = new BackupObject(backupObjectRep, @"/test2.txt");
        var backupObjects = new List<BackupObject> { backupObject1, backupObject2 };

        var backupTask = new BackupTask("test1 inMemory split", backupObjects, algorithm, storageRep, archiver);
        backupTask.Run();
        backupTask.Run();

        Assert.Equal(2, GetNumberOfRestorePoints(backupTask));
    }

    private static int GetNumberOfRestorePoints(BackupTask backupTask) => backupTask.Backup.BackupCopy.Count;
}