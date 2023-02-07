namespace Isu.Extra.Exceptions;

public class InvalidWeekDayException : IsuExtraException
{
    public InvalidWeekDayException()
        : base($"weekday must be a number between 1 and 7") { }
}