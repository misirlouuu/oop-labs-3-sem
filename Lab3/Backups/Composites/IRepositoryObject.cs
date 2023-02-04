using Backups.Visitors;

namespace Backups.Composites;

public interface IRepositoryObject
{
    void Accept(IRepositoryObjectVisitor visitor);
}