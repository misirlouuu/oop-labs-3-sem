using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public record LessonTime
{
    public LessonTime(int weekDay, DateTime startTime, DateTime endTime)
    {
        if (weekDay is < 1 or > 7)
            throw new InvalidWeekDayException();
        WeekDay = weekDay;

        Validate(startTime, endTime);
        StartTime = startTime;
        EndTime = endTime;
    }

    public int WeekDay { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }

    private static void Validate(DateTime startTime, DateTime endTime)
    {
        ArgumentNullException.ThrowIfNull(startTime);
        ArgumentNullException.ThrowIfNull(endTime);

        TimeSpan duration = endTime - startTime;

        if (!duration.TotalMinutes.Equals(90))
            throw new InvalidLessonDurationException();
    }
}