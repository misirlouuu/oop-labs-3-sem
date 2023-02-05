using Isu.Models;

namespace Isu.Entities;

public class Student
{
    private static int _id = 100000;

    public Student(string firstName, string lastName, Group group)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentNullException("firstName");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentNullException("lastName");
        }

        ArgumentNullException.ThrowIfNull(group);

        Group = group;
        FirstName = firstName;
        LastName = lastName;
        Id = _id++;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public Group Group { get; private set; }
    public int Id { get; }

    public void ChangeGroup(Group newGroup)
    {
        var oldGroup = Group;
        newGroup.AddStudent(this);
        oldGroup.RemoveStudent(this);
        Group = newGroup;
    }
}