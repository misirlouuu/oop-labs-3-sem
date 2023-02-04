using Backups.Composites;

namespace Backups.Interfaces;

public interface IRepositoryObjectVisitor
{
    void Visit(FileRepositoryObject file);
    void Visit(FolderRepositoryObject folder);
}