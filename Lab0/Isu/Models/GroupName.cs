using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    public GroupName(string groupName)
    {
        IsCorrectGroupName(groupName);
        Name = groupName;
        Faculty = groupName[0];
        int course = Convert.ToInt32(groupName[2]) - '0';
        Course = new CourseNumber(course);
    }

    public string Name { get; }
    public char Faculty { get; }
    public CourseNumber Course { get; }

    private static void IsCorrectGroupName(string groupName)
    {
        if (groupName.Length < 5)
        {
            throw new InvalidGroupNameException("Invalid groupName");
        }

        if (!char.IsLetter(groupName[0]))
        {
            throw new InvalidGroupNameException("Invalid groupName");
        }

        if (groupName[1..].Any(s => !char.IsDigit(s)))
        {
            throw new InvalidGroupNameException("Invalid groupName");
        }
    }
}