namespace Backups.Interfaces;

public interface IRepository
{
    string RootPath { get; }
    Stream OpenWrite(string path);
    IRepositoryObject GetRepositoryObject(string path);
    void ChangeRootDirectory(string path);
}