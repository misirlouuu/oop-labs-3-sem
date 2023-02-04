using Backups.Visitors;

namespace Backups.Composites;

public class FolderRepositoryObject : IRepositoryObject
{
    private readonly Func<IReadOnlyCollection<IRepositoryObject>> _children;

    public FolderRepositoryObject(Func<IReadOnlyCollection<IRepositoryObject>> factory, string name)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _children = factory;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyCollection<IRepositoryObject> Children => _children.Invoke();

    public void Accept(IRepositoryObjectVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}