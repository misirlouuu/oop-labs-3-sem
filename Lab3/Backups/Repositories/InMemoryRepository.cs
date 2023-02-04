using Backups.Composites;

namespace Backups.Repositories;

public class InMemoryRepository : IRepository
{
    public Stream OpenWrite(string relativePath)
    {
        throw new NotImplementedException();
    }

    public IRepositoryObject GetRepositoryObject(string relativePath)
    {
        throw new NotImplementedException();
    }

    public void ChangeRootDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public bool Exists(string? relativePath)
    {
        throw new NotImplementedException();
    }
}