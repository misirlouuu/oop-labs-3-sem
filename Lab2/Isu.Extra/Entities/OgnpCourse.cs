using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class OgnpCourse : IEquatable<OgnpCourse>
{
    private readonly List<OgnpGroup> _ognpGroups = new ();

    public OgnpCourse(string name, int capacity, MegaFaculty megaFaculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;

        if (capacity <= 0)
            throw new InvalidCountException();
        Capacity = capacity;

        ArgumentNullException.ThrowIfNull(megaFaculty);
        MegaFaculty = megaFaculty;

        Id = Guid.NewGuid();
    }

    public IReadOnlyCollection<OgnpGroup> OgnpGroups => _ognpGroups;
    public string Name { get; }
    public int Capacity { get; }
    public MegaFaculty MegaFaculty { get; }
    public Guid Id { get; }

    public OgnpGroup AddOgnpGroup(OgnpGroup ognpGroup)
    {
        ArgumentNullException.ThrowIfNull(ognpGroup);

        if (_ognpGroups.Contains(ognpGroup))
            throw new OgnpGroupAlreadyExistsException(ognpGroup);

        if (!HasEnoughPlaces(ognpGroup))
            throw new NotEnoughPlacesException();
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

    public bool HasEnoughPlaces(OgnpGroup ognpGroup) =>
        _ognpGroups.Sum(group => group.Capacity) + ognpGroup.Capacity < Capacity;

    public bool Equals(OgnpCourse? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((OgnpCourse)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_ognpGroups, Name, Capacity, MegaFaculty, Id);
    }
}