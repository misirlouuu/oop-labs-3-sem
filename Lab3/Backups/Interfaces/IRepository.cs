namespace Backups.Interfaces;

public interface IRepository
{
    string RootPath { get; }
    Stream OpenWrite(string relativePath);
    IRepositoryObject GetRepositoryObject(string relativePath);
    void ChangeRootDirectory(string path);
    bool Exists(string relativePath);
}