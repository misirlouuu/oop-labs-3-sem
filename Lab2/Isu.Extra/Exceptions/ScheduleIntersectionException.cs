namespace Isu.Extra.Exceptions;

public class ScheduleIntersectionException : IsuExtraException
{
    public ScheduleIntersectionException()
        : base($"schedules have time intersections") { }
}