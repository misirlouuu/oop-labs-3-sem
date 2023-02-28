using Backups.Entities;
using Backups.Models;
using Backups.Models.Algorithms;
using Backups.Repositories;
using Backups.Services;

namespace ConsoleApp1;

public static class Program
{
    public static void Main()
    {
        string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var repository = new FileSystemRepository(systemPath + Path.DirectorySeparatorChar + "repository1");
        var storageRepository = new FileSystemRepository(systemPath + Path.DirectorySeparatorChar + "repository2");
        
        var backupObject1 = new BackupObject(repository, @"problems1.pdf");
        var backupObject2 = new BackupObject(repository, @"testFolder");
        var backupObjects = new List<BackupObject> { backupObject1, backupObject2};
        
        var algorithm = new SingleStorageAlgorithm();
        var archiver = new Archiver();
        
        var backupTask = new BackupTask(" test12 single", backupObjects, algorithm, storageRepository, archiver);
        backupTask.Run();
    }
}