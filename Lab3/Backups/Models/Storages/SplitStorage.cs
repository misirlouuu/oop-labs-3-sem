using Backups.Interfaces;

namespace Backups.Models.Storages;

public class SplitStorage : IStorage
{
    private IReadOnlyCollection<ZipStorage> _storages;

    public SplitStorage(IRepository repository, IReadOnlyCollection<ZipStorage> storages)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(storages);

        Repository = repository;
        _storages = storages;
    }

    public IRepository Repository { get; }

    public IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects()
    {
        return _storages.SelectMany(storage => storage.GetRepositoryObjects()).ToArray();
    }
}