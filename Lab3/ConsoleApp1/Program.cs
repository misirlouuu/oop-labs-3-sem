using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
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
        
        var algorithm = new SplitStorageAlgorithm();
        var archiver = new Archiver();
        
        var backupTask = new BackupTask(" test8 split", backupObjects, algorithm, storageRepository, archiver);
        backupTask.Run();
        
        // const string path1 = @"/Users/ekaterina/Desktop/Backups";
        // const string path2 = @"/Users/ekaterina/Desktop/Backups.Tests";
        //
        // var repository = new FileSystemRepository(path1);
        // var storageRepository = new FileSystemRepository(path2);
        //
        // var algorithm = new SplitStorageAlgorithm();
        // var archiver = new Archiver();
        //
        // var backupObject1 = new BackupObject(repository, @"Backup objects/1.pdf");
        // var backupObject2 = new BackupObject(repository, @"Backup objects/f1");
        //
        // var backupObjects = new List<BackupObject> { backupObject1, backupObject2};
        //
        // var backupTask = new BackupTask("test2", backupObjects, algorithm, storageRepository, archiver);
        // backupTask.Run();
    }
}