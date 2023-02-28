using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models.Composites;
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
        {
            throw new ArgumentNullException(rootPath);
        }

        RootPath = rootPath;

        _fs = new MemoryFileSystem();

        _factory = Factory;
        _stream = OpenWrite;
    }

    public string RootPath { get; private set; }

    public Stream OpenWrite(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        UPath absolutePath = new UPath(path).ToAbsolute();
        _fs.CreateDirectory(absolutePath.GetDirectory());

        return _fs.OpenFile(absolutePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public IRepositoryObject GetRepositoryObject(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        UPath absolutePath = new UPath(path).ToAbsolute();

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
        {
            throw new ArgumentNullException(path);
        }

        RootPath = new UPath(path).ToAbsolute().ToString();
    }

    public void WriteAllText(string path, string text)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        UPath absolutePath = new UPath(path).ToAbsolute();

        _fs.WriteAllText(absolutePath, text);
    }

    public void Dispose()
    {
        _fs.Dispose();
    }

    private IReadOnlyCollection<IRepositoryObject> Factory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        string absolutePath = new UPath(path).ToAbsolute().ToString();

        var repositoryObjects = _fs.EnumerateDirectories(new UPath(absolutePath))
            .Select(upath => GetRepositoryObject(upath.ToString()))
            .ToList();

        repositoryObjects.AddRange(_fs.EnumerateFiles(new UPath(absolutePath))
            .Select(upath => GetRepositoryObject(upath.ToString())));

        return repositoryObjects;
    }
}