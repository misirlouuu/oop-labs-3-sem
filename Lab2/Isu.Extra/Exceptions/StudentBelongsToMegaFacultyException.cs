using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class StudentBelongsToMegaFacultyException : IsuExtraException
{
    public StudentBelongsToMegaFacultyException(IsuExtraStudent student)
        : base($"student {student.Student.FirstName} {student.Student.LastName} belongs to the same megafaculty") { }
}