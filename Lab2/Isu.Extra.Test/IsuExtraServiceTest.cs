using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private readonly IsuExtraService _service = new (2);

    [Fact]
    public void RegisterStudentOnCourse_StudentBelongsToMegaFaculty()
    {
        Group group = _service.IsuService.AddGroup(new GroupName("M32131"));
        Student isuStudent = _service.IsuService.AddStudent(group, "Dmitriy", "Vostretsov");

        var faculties = new List<char> { 'M', 'J', 'K' };
        MegaFaculty megaFaculty = _service.AddMegaFaculty("TINT", faculties);

        var lecturer1 = new Lecturer("technologies of programming", "Roman Makarevich");

        var start1 = new DateTime(2022, 2, 9, 10, 0, 0);
        var end1 = new DateTime(2022, 2, 9, 11, 30, 0);
        var lessonTime1 = new LessonTime(4, start1, end1);
        var lesson1 = new Lesson(lessonTime1, 2304, lecturer1);

        var start2 = new DateTime(2022, 2, 9, 11, 40, 0);
        var end2 = new DateTime(2022, 2, 9, 13, 10, 0);
        var lessonTime2 = new LessonTime(4, start2, end2);
        var lesson2 = new Lesson(lessonTime2, 2304, lecturer1);

        var lessons1 = new List<Lesson> { lesson1, lesson2 };

        var lecturer2 = new Lecturer("applied mathematics", "Maria Moskalenko");

        var start3 = new DateTime(2022, 2, 9, 10, 0, 0);
        var end3 = new DateTime(2022, 2, 9, 11, 30, 0);
        var lessonTime3 = new LessonTime(5, start3, end3);
        var lesson3 = new Lesson(lessonTime3, 2342, lecturer2);

        var start4 = new DateTime(2022, 2, 9, 11, 40, 0);
        var end4 = new DateTime(2022, 2, 9, 13, 10, 0);
        var lessonTime4 = new LessonTime(5, start4, end4);
        var lesson4 = new Lesson(lessonTime4, 2342, lecturer2);

        var lessons2 = new List<Lesson> { lesson3, lesson4 };

        var student = _service.AddIsuExtraStudent(lessons1, group, isuStudent);

        var name = new OgnpGroupName("nnn111");
        var name2 = new OgnpGroupName("nnn222");
        var course = _service.AddOgnpCourse("UNIX", 600, megaFaculty, lessons2, name, 35);
        var group2 = _service.AddOgnpGroup(name, lessons2, course, 35);

        Assert.Throws<StudentBelongsToMegaFacultyException>(() => _service.RegisterStudentOnCourse(course, student, group2));
    }

    [Fact]
    public void RegisterStudentOnCourse_StudentHasGroupAndGroupContainsStudent()
    {
        Group group = _service.IsuService.AddGroup(new GroupName("M32131"));
        Student isuStudent = _service.IsuService.AddStudent(group, "Dmitriy", "Vostretsov");
        Student isuStudent2 = _service.IsuService.AddStudent(group, "Nikolay", "Shishelyakin");

        var faculties = new List<char> { 'P', 'R', 'N', 'H' };
        MegaFaculty megaFaculty = _service.AddMegaFaculty("CTU", faculties);

        var lecturer1 = new Lecturer("technologies of programming", "Roman Makarevich");

        var start1 = new DateTime(2022, 2, 9, 10, 0, 0);
        var end1 = new DateTime(2022, 2, 9, 11, 30, 0);
        var lessonTime1 = new LessonTime(4, start1, end1);
        var lesson1 = new Lesson(lessonTime1, 2304, lecturer1);

        var start2 = new DateTime(2022, 2, 9, 11, 40, 0);
        var end2 = new DateTime(2022, 2, 9, 13, 10, 0);
        var lessonTime2 = new LessonTime(4, start2, end2);
        var lesson2 = new Lesson(lessonTime2, 2304, lecturer1);

        var lessons1 = new List<Lesson> { lesson1, lesson2 };

        var lecturer2 = new Lecturer("applied mathematics", "Maria Moskalenko");

        var start3 = new DateTime(2022, 2, 9, 10, 0, 0);
        var end3 = new DateTime(2022, 2, 9, 11, 30, 0);
        var lessonTime3 = new LessonTime(5, start3, end3);
        var lesson3 = new Lesson(lessonTime3, 2342, lecturer2);

        var start4 = new DateTime(2022, 2, 9, 11, 40, 0);
        var end4 = new DateTime(2022, 2, 9, 13, 10, 0);
        var lessonTime4 = new LessonTime(5, start4, end4);
        var lesson4 = new Lesson(lessonTime4, 2342, lecturer2);

        var lessons2 = new List<Lesson> { lesson3, lesson4 };

        var student = _service.AddIsuExtraStudent(lessons1, group, isuStudent);
        var student2 = _service.AddIsuExtraStudent(lessons1, group, isuStudent2);

        var name = new OgnpGroupName("nnn111");
        var course = _service.AddOgnpCourse("cybersecurity", 800, megaFaculty, lessons2, name, 1);

        var name2 = new OgnpGroupName("nnn222");
        var group2 = _service.AddOgnpGroup(name2, lessons2, course, 10);
        _service.RegisterStudentOnCourse(course, student, group2);

        Assert.Contains(student, group2.Students);
    }

    [Fact]
    public void RegisterStudentOnCourse_SchedulesIntersect()
    {
        Group group = _service.IsuService.AddGroup(new GroupName("M32131"));
        Student isuStudent = _service.IsuService.AddStudent(group, "Dmitriy", "Vostretsov");

        var faculties = new List<char> { 'U' };
        MegaFaculty megaFaculty = _service.AddMegaFaculty("FTMI", faculties);

        var lecturer1 = new Lecturer("technologies of programming", "Roman Makarevich");

        var start1 = new DateTime(2022, 2, 9, 10, 0, 0);
        var end1 = new DateTime(2022, 2, 9, 11, 30, 0);
        var lessonTime1 = new LessonTime(4, start1, end1);
        var lesson1 = new Lesson(lessonTime1, 2304, lecturer1);

        var start2 = new DateTime(2022, 2, 9, 11, 40, 0);
        var end2 = new DateTime(2022, 2, 9, 13, 10, 0);
        var lessonTime2 = new LessonTime(4, start2, end2);
        var lesson2 = new Lesson(lessonTime2, 2304, lecturer1);

        var lessons1 = new List<Lesson> { lesson1, lesson2 };

        var student = _service.AddIsuExtraStudent(lessons1, group, isuStudent);

        var name = new OgnpGroupName("nnn111");
        var name2 = new OgnpGroupName("nnn222");
        var course = _service.AddOgnpCourse("UNIX", 600, megaFaculty, lessons1, name, 35);
        var group2 = _service.AddOgnpGroup(name2, lessons1, course, 35);

        Assert.Throws<ScheduleIntersectionException>(() => _service.RegisterStudentOnCourse(course, student, group2));
    }
}