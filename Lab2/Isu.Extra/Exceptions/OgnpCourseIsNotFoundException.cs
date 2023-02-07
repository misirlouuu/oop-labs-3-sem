using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class OgnpCourseIsNotFoundException : IsuExtraException
{
    public OgnpCourseIsNotFoundException(OgnpCourse course)
        : base($"ognp course {course.Name} is not found") { }
}