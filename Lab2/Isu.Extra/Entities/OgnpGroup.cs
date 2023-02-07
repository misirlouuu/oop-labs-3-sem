using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class OgnpGroup : IEquatable<OgnpGroup>
{
    private readonly List<IsuExtraStudent> _students = new ();

    public OgnpGroup(OgnpGroupName groupName, Schedule schedule, OgnpCourse course, int capacity)
    {
        ArgumentNullException.ThrowIfNull(groupName);
        GroupName = groupName;

        ArgumentNullException.ThrowIfNull(schedule);
        Schedule = schedule;

        ArgumentNullException.ThrowIfNull(course);
        Course = course;

        if (capacity <= 0)
            throw new InvalidCountException();
        Capacity = capacity;

        Id = Guid.NewGuid();
    }

    public OgnpGroupName GroupName { get; }
    public Schedule Schedule { get; }
    public OgnpCourse Course { get; }
    public Guid Id { get; }
    public int Capacity { get; }
    public IReadOnlyCollection<IsuExtraStudent> Students => _students;

    public IsuExtraStudent AddStudent(IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (_students.Contains(student))
            throw new StudentAlreadyExistsException(student);

        if (HasIntersectionsWithMainSchedule(student) || HasIntersectionsWithOgnpSchedule(student))
            throw new ScheduleIntersectionException();

        student.AddOgnpGroup(this);
        _students.Add(student);

        return student;
    }

    public void RemoveStudent(IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (!_students.Contains(student))
            throw new StudentIsNotFoundException(student);

        student.RemoveOgnpGroup(this);
        _students.Remove(student);
    }

    public bool Equals(OgnpGroup? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((OgnpGroup)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_students, GroupName, Schedule, Course, Id, Capacity);
    }

    public bool HasEnoughPlaces() => _students.Count < Capacity;

    private bool HasIntersectionsWithMainSchedule(IsuExtraStudent student)
    {
        return Schedule.HasIntersections(student.Group.Schedule);
    }

    private bool HasIntersectionsWithOgnpSchedule(IsuExtraStudent student)
    {
        return student.OgnpGroups.Any(ognpGroup => ognpGroup.Schedule.HasIntersections(Schedule));
    }
}