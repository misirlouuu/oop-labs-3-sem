using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models.Composites;

namespace Backups.Repositories;

public class FileSystemRepository : IRepository
{
    private readonly Func<string, IReadOnlyCollection<IRepositoryObject>> _factory;
    private readonly Func<string, Stream> _stream;

    public FileSystemRepository(string rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            throw new ArgumentNullException(rootPath);
        }

        RootPath = rootPath;

        _factory = Factory;
        _stream = OpenWrite;
    }

    public string RootPath { get; private set; }

    public void ChangeRootDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        RootPath = Path.GetFullPath(path, RootPath);
    }

    public Stream OpenWrite(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        string absolutePath = Path.GetFullPath(path, RootPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? string.Empty);

        return new FileStream(absolutePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public IRepositoryObject GetRepositoryObject(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        string absolutePath = Path.GetFullPath(path, RootPath);

        if (File.Exists(absolutePath))
        {
            return new FileRepositoryObject(() => _stream(absolutePath), Path.GetFileName(absolutePath));
        }

        if (Directory.Exists(absolutePath))
        {
            return new FolderRepositoryObject(() => _factory(absolutePath), new DirectoryInfo(absolutePath).Name);
        }

        throw new BackupObjectIsNotFoundException();
    }

    private IReadOnlyCollection<IRepositoryObject> Factory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        string absolutePath = Path.GetFullPath(path, RootPath);

        var repositoryObjects = Directory.GetDirectories(absolutePath)
            .Select(GetRepositoryObject)
            .ToList();

        repositoryObjects.AddRange(Directory.GetFiles(absolutePath).Select(GetRepositoryObject));

        return repositoryObjects;
    }
}