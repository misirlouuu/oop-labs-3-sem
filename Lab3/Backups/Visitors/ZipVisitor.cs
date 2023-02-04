using System.IO.Compression;
using Backups.Composites;
using ZipFile = Backups.Composites.ZipFile;

namespace Backups.Visitors;

public class ZipVisitor : IRepositoryObjectVisitor
{
    private readonly Stack<ZipArchive> _zipArchives;
    private readonly Stack<List<IZipObject>> _zipObjects;

    public ZipVisitor(ZipArchive archive)
    {
        _zipArchives = new Stack<ZipArchive>();
        _zipArchives.Push(archive);

        _zipObjects = new Stack<List<IZipObject>>();
        _zipObjects.Push(new List<IZipObject>());
    }

    public Stack<List<IZipObject>> ZipObjects => _zipObjects;

    public void Visit(FileRepositoryObject file)
    {
        ArgumentNullException.ThrowIfNull(file);

        ZipArchive zipArchive = _zipArchives.Peek();
        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(file.Name);
        using Stream stream = zipArchiveEntry.Open();
        using Stream fileStream = file.Stream;
        fileStream.CopyTo(stream);

        var zipFile = new ZipFile(file.Name);
        _zipObjects.Peek().Add(zipFile);
    }

    public void Visit(FolderRepositoryObject folder)
    {
        ArgumentNullException.ThrowIfNull(folder);

        var zipFolder = new ZipFolder(new List<IZipObject>(_zipObjects.Pop()), folder.Name);
        ZipArchive zipArchive = _zipArchives.Peek();
        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(folder.Name + ".zip");
        using Stream stream = zipArchiveEntry.Open();

        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        _zipArchives.Push(archive);
        _zipObjects.Push(new List<IZipObject>());

        foreach (var repositoryObject in folder.Children)
        {
            repositoryObject.Accept(this);
        }

        // folder.RepositoryObjects.ToList().ForEach(repositoryObject => repositoryObject.Accept(this));
        _zipObjects.Peek().Add(zipFolder);
        _zipArchives.Pop();
    }
}