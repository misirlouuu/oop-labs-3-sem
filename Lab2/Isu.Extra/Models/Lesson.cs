using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Microsoft.VisualBasic;

namespace Isu.Extra.Models;

public record Lesson
{
    public Lesson(LessonTime lessonTime, int classroom, Lecturer lecturer)
    {
        ArgumentNullException.ThrowIfNull(lessonTime);
        LessonTime = lessonTime;

        if (classroom <= 0)
            throw new InvalidCountException();
        Classroom = classroom;

        ArgumentNullException.ThrowIfNull(lecturer);
        Lecturer = lecturer;
    }

    public LessonTime LessonTime { get; }
    public int Classroom { get; }
    public Lecturer Lecturer { get; }

    public bool HasIntersection(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        return LessonTime.Equals(lesson.LessonTime);
    }
}