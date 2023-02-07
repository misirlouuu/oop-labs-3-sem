using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class CourseHasAlreadyBeenChosenException : IsuExtraException
{
    public CourseHasAlreadyBeenChosenException(OgnpCourse course)
        : base($"course {course.Name} has already been chosen") { }
}