namespace Isu.Extra.Exceptions;

public class InvalidLessonDurationException : IsuExtraException
{
    public InvalidLessonDurationException()
        : base($"lesson must last 90 minutes") { }
}