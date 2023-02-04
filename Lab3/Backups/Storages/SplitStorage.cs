using Backups.Composites;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Storages;

public class SplitStorage : IStorage
{
    private IReadOnlyCollection<ZipStorage> _storages;

    public SplitStorage(IRepository repository, IReadOnlyCollection<ZipStorage> storages)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;

        ArgumentNullException.ThrowIfNull(storages);
        _storages = storages;
    }

    public IRepository Repository { get; }

    public IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects()
    {
        return _storages.SelectMany(storage => storage.GetRepositoryObjects()).ToArray();
    }
}