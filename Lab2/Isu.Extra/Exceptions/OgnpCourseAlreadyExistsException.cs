using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class OgnpCourseAlreadyExistsException : IsuExtraException
{
    public OgnpCourseAlreadyExistsException(OgnpCourse course)
        : base($"ognp course {course.Name} already exists") { }
}