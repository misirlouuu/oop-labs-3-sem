using System.IO.Compression;
using Backups.Interfaces;
using Backups.Models.Composites;
using ZipFile = Backups.Models.Composites.ZipFile;

namespace Backups.Models;

public class ZipVisitor : IRepositoryObjectVisitor
{
    private readonly Stack<ZipArchive> _zipArchives;

    public ZipVisitor(ZipArchive archive)
    {
        _zipArchives = new Stack<ZipArchive>();
        _zipArchives.Push(archive);

        ZipObjects = new Stack<List<IZipObject>>();
        ZipObjects.Push(new List<IZipObject>());
    }

    public Stack<List<IZipObject>> ZipObjects { get; }

    public void Visit(FileRepositoryObject file)
    {
        ArgumentNullException.ThrowIfNull(file);

        ZipArchive zipArchive = _zipArchives.Peek();
        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(file.Name);

        using Stream stream = zipArchiveEntry.Open();
        using Stream fileStream = file.Stream;
        fileStream.CopyTo(stream);

        var zipFile = new ZipFile(file.Name);
        ZipObjects.Peek().Add(zipFile);
    }

    public void Visit(FolderRepositoryObject folder)
    {
        ArgumentNullException.ThrowIfNull(folder);

        var zipFolder = new ZipFolder(new List<IZipObject>(ZipObjects.Pop()), folder.Name);

        ZipArchive zipArchive = _zipArchives.Peek();
        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(folder.Name + ".zip");
        using Stream stream = zipArchiveEntry.Open();

        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        _zipArchives.Push(archive);
        ZipObjects.Push(new List<IZipObject>());

        folder.Children.ToList().ForEach(repositoryObject => repositoryObject.Accept(this));

        ZipObjects.Peek().Add(zipFolder);
        _zipArchives.Pop();
    }
}