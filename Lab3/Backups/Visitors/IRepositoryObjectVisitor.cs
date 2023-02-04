using Backups.Composites;

namespace Backups.Visitors;

public interface IRepositoryObjectVisitor
{
    void Visit(FileRepositoryObject file);
    void Visit(FolderRepositoryObject folder);
}