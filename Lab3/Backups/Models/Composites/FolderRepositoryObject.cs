using Backups.Interfaces;

namespace Backups.Models.Composites;

public class FolderRepositoryObject : IRepositoryObject
{
    private readonly Func<IReadOnlyCollection<IRepositoryObject>> _factory;

    public FolderRepositoryObject(Func<IReadOnlyCollection<IRepositoryObject>> factory, string name)
    {
        ArgumentNullException.ThrowIfNull(factory);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(name);
        }

        _factory = factory;
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