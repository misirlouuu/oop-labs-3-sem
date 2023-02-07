namespace Isu.Extra.Exceptions;

public class MaxAmountOfCoursesWasReachedException : IsuExtraException
{
    public MaxAmountOfCoursesWasReachedException(int count)
        : base($"max amount of course ({count}) was reached") { }
}