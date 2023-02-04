using Backups.Composites;

namespace Backups.Storages;

public interface IStorage
{
    IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects();
}