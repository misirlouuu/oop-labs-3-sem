namespace Backups.Interfaces;

public interface IStorage
{
    IReadOnlyCollection<IRepositoryObject> GetRepositoryObjects();
}