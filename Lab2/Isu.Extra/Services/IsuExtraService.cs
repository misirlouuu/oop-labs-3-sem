using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private readonly List<IsuExtraStudent> _students = new ();
    private readonly List<OgnpGroup> _ognpGroups = new ();
    private readonly List<OgnpCourse> _courses = new ();
    private readonly List<MegaFaculty> _megaFaculties = new ();
    private readonly IsuService _service = new ();

    public IsuExtraService(int maxAmountOfCourses)
    {
        if (maxAmountOfCourses <= 0)
            throw new InvalidCountException();
        MaxAmountOfCourses = maxAmountOfCourses;
    }

    public int MaxAmountOfCourses { get; }
    public IsuService IsuService => _service;

    public MegaFaculty AddMegaFaculty(string name, List<char> facultyLetters)
    {
        var megaFaculty = new MegaFaculty(name, facultyLetters);

        if (_megaFaculties.Contains(megaFaculty))
            throw new MegaFacultyAlreadyExistsException(megaFaculty);
        _megaFaculties.Add(megaFaculty);

        return megaFaculty;
    }

    public OgnpCourse AddOgnpCourse(
        string name,
        int courseCapacity,
        MegaFaculty megaFaculty,
        List<Lesson> lessons,
        OgnpGroupName groupName,
        int groupCapacity)
    {
        var course = new OgnpCourse(name, courseCapacity, megaFaculty);
        AddOgnpGroup(groupName, lessons, course, groupCapacity);

        if (_courses.Contains(course))
            throw new OgnpCourseAlreadyExistsException(course);
        _courses.Add(course);

        return course;
    }

    public OgnpGroup AddOgnpGroup(OgnpGroupName groupName, List<Lesson> lessons, OgnpCourse course, int capacity)
    {
        var schedule = GetSchedule(lessons);
        var ognpGroup = new OgnpGroup(groupName, schedule, course, capacity);

        if (_ognpGroups.Contains(ognpGroup))
            throw new OgnpGroupAlreadyExistsException(ognpGroup);

        course.AddOgnpGroup(ognpGroup);
        _ognpGroups.Add(ognpGroup);

        return ognpGroup;
    }

    public IsuExtraStudent AddIsuExtraStudent(List<Lesson> lessons, Group group, Student student)
    {
        var schedule = GetSchedule(lessons);
        var isuExtraGroup = new IsuExtraGroup(schedule, group);
        var isuExtraStudent = new IsuExtraStudent(isuExtraGroup, student);

        if (_students.Contains(isuExtraStudent))
            throw new StudentAlreadyExistsException(isuExtraStudent);
        _students.Add(isuExtraStudent);

        return isuExtraStudent;
    }

    public IsuExtraStudent RegisterStudentOnCourse(OgnpCourse course, IsuExtraStudent student, OgnpGroup group)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(group);

        if (!_students.Contains(student))
            throw new StudentIsNotFoundException(student);

        if (BelongsToMegaFaculty(course, student))
            throw new StudentBelongsToMegaFacultyException(student);

        if (HasCourseAlreadyChosen(course, student))
            throw new CourseHasAlreadyBeenChosenException(course);

        if (HasMaxAmountOfCoursesReached(student))
            throw new MaxAmountOfCoursesWasReachedException(MaxAmountOfCourses);

        if (!_ognpGroups.Contains(group) || !course.OgnpGroups.Contains(group))
            throw new OgnpGroupIsNotFoundException(group);

        return group.AddStudent(student);
    }

    public void RemoveStudentFromCourse(OgnpCourse course, IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(student);

        if (!_students.Contains(student))
            throw new StudentIsNotFoundException(student);

        OgnpGroup? group = FindGroup(course, student);

        if (group is null)
            throw new OgnpGroupIsNotFoundException();
        group.RemoveStudent(student);
    }

    public IReadOnlyCollection<OgnpGroup> FindGroups(OgnpCourse course)
    {
        ArgumentNullException.ThrowIfNull(course);

        return _courses.Contains(course) ? course.OgnpGroups : new List<OgnpGroup>();
    }

    public IReadOnlyCollection<IsuExtraStudent> FindStudents(OgnpGroup group)
    {
        ArgumentNullException.ThrowIfNull(group);

        return _ognpGroups.Contains(group) ? group.Students : new List<IsuExtraStudent>();
    }

    public IReadOnlyCollection<IsuExtraStudent> FindUnregisteredStudents(IsuExtraGroup group)
    {
        ArgumentNullException.ThrowIfNull(group);

        return _students.FindAll(student => student.Group.Equals(group))
            .FindAll(student => student.OgnpGroups.Count == 0);
    }

    private Schedule GetSchedule(IReadOnlyCollection<Lesson> lessons)
    {
        ArgumentNullException.ThrowIfNull(lessons);

        var builder = new Schedule.ScheduleBuilder();
        foreach (Lesson lesson in lessons)
            builder.WithLesson(lesson);

        return builder.Build();
    }

    private OgnpGroup? FindGroup(OgnpCourse course, IsuExtraStudent student)
    {
        return course.OgnpGroups.FirstOrDefault(group => group.Students.Contains(student));
    }

    private bool HasMaxAmountOfCoursesReached(IsuExtraStudent student)
    {
        return student.OgnpGroups.Count >= MaxAmountOfCourses;
    }

    private bool HasCourseAlreadyChosen(OgnpCourse course, IsuExtraStudent student)
    {
        return student.OgnpGroups.Any(ognpGroup => ognpGroup.Course.Equals(course));
    }

    private bool BelongsToMegaFaculty(OgnpCourse course, IsuExtraStudent student)
    {
        var studentsFaculty = student.Group.IsuGroup.GroupName.Faculty;
        return course.MegaFaculty.ContainsFaculty(studentsFaculty);
    }
}