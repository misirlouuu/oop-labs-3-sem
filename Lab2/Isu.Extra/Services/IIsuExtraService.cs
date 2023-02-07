using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
    MegaFaculty AddMegaFaculty(string name, List<char> facultyLetters);

    OgnpCourse AddOgnpCourse(
        string name,
        int courseCapacity,
        MegaFaculty megaFaculty,
        List<Lesson> lessons,
        OgnpGroupName groupName,
        int groupCapacity);

    OgnpGroup AddOgnpGroup(OgnpGroupName groupName, List<Lesson> lessons, OgnpCourse course, int capacity);
    IsuExtraStudent AddIsuExtraStudent(List<Lesson> lessons, Group group, Student student);
    IsuExtraStudent RegisterStudentOnCourse(OgnpCourse course, IsuExtraStudent student, OgnpGroup group);
    void RemoveStudentFromCourse(OgnpCourse course, IsuExtraStudent student);

    IReadOnlyCollection<OgnpGroup> FindGroups(OgnpCourse course);
    IReadOnlyCollection<IsuExtraStudent> FindStudents(OgnpGroup group);
    IReadOnlyCollection<IsuExtraStudent> FindUnregisteredStudents(IsuExtraGroup group);
}