using Backups.Composites;
using Backups.Exceptions;
using Backups.Interfaces;
using Zio;
using Zio.FileSystems;

namespace Backups.Repositories;

public class InMemoryRepository : IDisposable, IRepository
{
    private readonly MemoryFileSystem _fs;
    private readonly Func<string, IReadOnlyCollection<IRepositoryObject>> _factory;
    private readonly Func<string, Stream> _stream;

    public InMemoryRepository(string rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
            throw new ArgumentNullException(rootPath);
        RootPath = rootPath;

        _fs = new MemoryFileSystem();

        _factory = Factory;
        _stream = OpenWrite;
    }

    public string RootPath { get; private set; }

    public Stream OpenWrite(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);

        UPath absolutePath = new UPath(relativePath).ToAbsolute();
        _fs.CreateDirectory(absolutePath.GetDirectory());

        return _fs.OpenFile(absolutePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public IRepositoryObject GetRepositoryObject(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);

        UPath absolutePath = new UPath(relativePath).ToAbsolute();

        if (_fs.FileExists(absolutePath))
        {
            string name = _fs.GetFileEntry(absolutePath).Name;
            return new FileRepositoryObject(() => _stream(absolutePath.ToString()), name);
        }

        if (_fs.DirectoryExists(absolutePath))
        {
            string name = _fs.GetDirectoryEntry(absolutePath).Name;
            return new FolderRepositoryObject(() => _factory(absolutePath.ToString()), name);
        }

        throw new BackupObjectIsNotFoundException();
    }

    public void ChangeRootDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);
        RootPath = Path.GetFullPath(path, RootPath);
    }

    public bool Exists(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);
        UPath absolutePath = new UPath(relativePath).ToAbsolute();

        return _fs.DirectoryExists(absolutePath.GetDirectory());
    }

    public void WriteAllText(string relativePath, string text)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);
        UPath absolutePath = new UPath(relativePath).ToAbsolute();

        _fs.WriteAllText(absolutePath, text);
    }

    public void Dispose()
    {
        _fs.Dispose();
    }

    private IReadOnlyCollection<IRepositoryObject> Factory(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);

        string absolutePath = Path.GetFullPath(relativePath, RootPath);

        var repositoryObjects = _fs.EnumerateDirectories(new UPath(absolutePath))
            .Select(upath => GetRepositoryObject(upath.ToString()))
            .ToList();

        repositoryObjects.AddRange(_fs.EnumerateFiles(new UPath(absolutePath))
            .Select(upath => GetRepositoryObject(upath.ToString())));

        return repositoryObjects;
    }
}