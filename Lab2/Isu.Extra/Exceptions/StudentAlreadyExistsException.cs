using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class StudentAlreadyExistsException : IsuExtraException
{
    public StudentAlreadyExistsException(IsuExtraStudent student)
        : base($"student {student.Student.FirstName} {student.Student.LastName} with id {student.Id} already exists") { }
}