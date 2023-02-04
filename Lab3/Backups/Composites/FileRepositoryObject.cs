using Backups.Visitors;

namespace Backups.Composites;

public class FileRepositoryObject : IRepositoryObject
{
    private readonly Func<Stream> _stream;
    public FileRepositoryObject(Func<Stream> stream, string name)
    {
        _stream = stream;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;
    }

    public Stream Stream => _stream.Invoke();
    public string Name { get; }

    public void Accept(IRepositoryObjectVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}