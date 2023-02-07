using Isu.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class IsuExtraGroup : IEquatable<IsuExtraGroup>
{
    public IsuExtraGroup(Schedule schedule, Group isuGroup)
    {
        ArgumentNullException.ThrowIfNull(schedule);
        Schedule = schedule;

        ArgumentNullException.ThrowIfNull(isuGroup);
        IsuGroup = isuGroup;

        Id = Guid.NewGuid();
    }

    public Schedule Schedule { get; }
    public Group IsuGroup { get; }
    public Guid Id { get; }

    public bool Equals(IsuExtraGroup? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((IsuExtraGroup)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Schedule, IsuGroup, Id);
    }
}