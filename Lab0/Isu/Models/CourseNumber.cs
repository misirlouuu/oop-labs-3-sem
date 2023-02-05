using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    public CourseNumber(int course)
    {
        if (course > 4)
        {
            throw new IsuException("Invalid courseNumber");
        }

        Course = course;
    }

    public int Course { get; }
}