using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class MegaFaculty : IEquatable<MegaFaculty>
{
    private readonly List<OgnpCourse> _ognpCourses = new ();
    private readonly List<char> _facultyLetters;

    public MegaFaculty(string name, List<char> facultyLetters)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;

        ArgumentNullException.ThrowIfNull(facultyLetters);
        _facultyLetters = facultyLetters;

        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }

    public OgnpCourse AddOgnpCourse(OgnpCourse ognpCourse)
    {
        ArgumentNullException.ThrowIfNull(ognpCourse);

        if (_ognpCourses.Contains(ognpCourse))
            throw new OgnpCourseAlreadyExistsException(ognpCourse);
        _ognpCourses.Add(ognpCourse);

        return ognpCourse;
    }

    public void RemoveOgnpCourse(OgnpCourse ognpCourse)
    {
        ArgumentNullException.ThrowIfNull(ognpCourse);

        if (!_ognpCourses.Contains(ognpCourse))
            throw new OgnpCourseIsNotFoundException(ognpCourse);
        _ognpCourses.Remove(ognpCourse);
    }

    public bool ContainsFaculty(char faculty) => _facultyLetters.Contains(faculty);

    public bool Equals(MegaFaculty? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((MegaFaculty)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_ognpCourses, _facultyLetters, Name, Id);
    }
}