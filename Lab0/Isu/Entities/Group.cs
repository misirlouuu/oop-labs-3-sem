using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;
public class Group
{
    private const int MaxAmountOfStudents = 40;
    public Group(GroupName groupName)
    {
        ArgumentNullException.ThrowIfNull(groupName);
        GroupName = groupName;
        Students = new List<Student>();
    }

    public GroupName GroupName { get; }
    public List<Student> Students { get; }

    public void AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (Students.Contains(student))
        {
            throw new IsuException($"{student} already contains in group");
        }

        if (Students.Count >= MaxAmountOfStudents)
        {
            throw new ReachMaxStudentPerGroupException("reach max amount of students in group");
        }

        Students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (!Students.Contains(student))
        {
            throw new IsuException("no such student in this group");
        }

        Students.Remove(student);
    }
}