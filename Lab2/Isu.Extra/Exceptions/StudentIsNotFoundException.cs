using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class StudentIsNotFoundException : IsuExtraException
{
    public StudentIsNotFoundException(IsuExtraStudent student)
        : base($"student {student.Student.FirstName} {student.Student.LastName} with id {student.Id} is not found") { }
}