using Backups.Algorithms;
using Backups.Composites;
using Backups.Repositories;

namespace Backups.Storages;

public class ZipStorage : IStorage
{
    public ZipStorage(IRepository repository, ZipFolder zipFolder)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;

        ArgumentNullException.ThrowIfNull(zipFolder);
        ZipFolder = zipFolder;
    }

    public IRepository Repository { get; }
    public ZipFolder ZipFolder { get; }

    public IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects()
    {
        throw new NotImplementedException();
    }
}