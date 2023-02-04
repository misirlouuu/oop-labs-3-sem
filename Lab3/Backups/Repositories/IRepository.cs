using Backups.Composites;

namespace Backups.Repositories;

public interface IRepository
{
    Stream OpenWrite(string relativePath);
    IRepositoryObject GetRepositoryObject(string relativePath);
    void ChangeRootDirectory(string path);
    bool Exists(string relativePath);
}