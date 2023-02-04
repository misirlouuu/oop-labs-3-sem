using Backups.Interfaces;
using Backups.Visitors;

namespace Backups.Composites;

public class FolderRepositoryObject : IRepositoryObject
{
    private readonly Func<IReadOnlyCollection<IRepositoryObject>> _factory;

    public FolderRepositoryObject(Func<IReadOnlyCollection<IRepositoryObject>> factory, string name)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;
    }

    public string Name { get; }
    public IEnumerable<IRepositoryObject> Children => _factory.Invoke();

    public void Accept(IRepositoryObjectVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}