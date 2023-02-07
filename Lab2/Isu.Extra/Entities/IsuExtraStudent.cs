using Isu.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class IsuExtraStudent : IEquatable<IsuExtraStudent>
{
    private readonly List<OgnpGroup> _ognpGroups;

    public IsuExtraStudent(IsuExtraGroup group, Student student)
    {
        _ognpGroups = new List<OgnpGroup>();

        ArgumentNullException.ThrowIfNull(student);
        Student = student;

        ArgumentNullException.ThrowIfNull(group);
        Group = group;

        Id = Guid.NewGuid();
    }

    public IsuExtraGroup Group { get; }
    public Student Student { get; }
    public Guid Id { get; }
    public IReadOnlyCollection<OgnpGroup> OgnpGroups => _ognpGroups;

    public OgnpGroup AddOgnpGroup(OgnpGroup ognpGroup)
    {
        ArgumentNullException.ThrowIfNull(ognpGroup);

        if (_ognpGroups.Contains(ognpGroup))
            throw new OgnpGroupAlreadyExistsException(ognpGroup);

        _ognpGroups.Add(ognpGroup);

        return ognpGroup;
    }

    public void RemoveOgnpGroup(OgnpGroup ognpGroup)
    {
        ArgumentNullException.ThrowIfNull(ognpGroup);

        if (!_ognpGroups.Contains(ognpGroup))
            throw new OgnpGroupIsNotFoundException(ognpGroup);

        _ognpGroups.Remove(ognpGroup);
    }

    public bool Equals(IsuExtraStudent? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((IsuExtraStudent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_ognpGroups, Group, Student, Id);
    }
}