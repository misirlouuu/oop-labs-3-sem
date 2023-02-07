using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Schedule
{
    private Schedule(IReadOnlyCollection<Lesson> lessons)
    {
        ArgumentNullException.ThrowIfNull(lessons);
        Lessons = lessons;
    }

    public static ScheduleBuilder Builder => new ScheduleBuilder();
    public IReadOnlyCollection<Lesson> Lessons { get; }

    public bool HasIntersections(Schedule schedule)
    {
        return Lessons.Any(lesson => schedule.Lessons.Any(lesson.HasIntersection));
    }

    public class ScheduleBuilder
    {
        private readonly List<Lesson> _lessons = new ();

        public ScheduleBuilder WithLesson(Lesson newLesson)
        {
            ArgumentNullException.ThrowIfNull(newLesson);

            if (_lessons.Any(lesson => lesson.LessonTime.Equals(newLesson.LessonTime)))
                throw new ScheduleIntersectionException();
            _lessons.Add(newLesson);

            return this;
        }

        public Schedule Build() => new Schedule(_lessons);
    }
}