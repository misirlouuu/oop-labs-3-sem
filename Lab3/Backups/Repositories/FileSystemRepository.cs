using Backups.Composites;
using Backups.Exceptions;

namespace Backups.Repositories;

public class FileSystemRepository : IRepository
{
    private readonly Func<string, IReadOnlyCollection<IRepositoryObject>> _factory;
    private readonly Func<string, Stream> _stream;
    private string _rootPath;

    public FileSystemRepository(string rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
            throw new ArgumentNullException(rootPath);
        _rootPath = rootPath;

        _factory = Factory;
        _stream = Stream;
    }

    public void ChangeRootDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);
        _rootPath = Path.GetFullPath(path, _rootPath);
    }

    public bool Exists(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);
        string absolutePath = Path.GetFullPath(relativePath, _rootPath);

        return Directory.Exists(Path.GetDirectoryName(absolutePath));
    }

    public Stream OpenWrite(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);

        string absolutePath = Path.GetFullPath(relativePath, _rootPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? string.Empty);

        return new FileStream(absolutePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public IRepositoryObject GetRepositoryObject(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);

        string absolutePath = Path.GetFullPath(relativePath, _rootPath);

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

    private Stream Stream(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);

        return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    private IReadOnlyCollection<IRepositoryObject> Factory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);

        var repositoryObjects = Directory.GetDirectories(path)
            .Select(GetRepositoryObject)
            .ToList();

        repositoryObjects.AddRange(Directory.GetFiles(path).Select(GetRepositoryObject));

        return repositoryObjects;
    }
}