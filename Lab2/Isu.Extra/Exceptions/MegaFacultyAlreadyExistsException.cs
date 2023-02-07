using Isu.Extra.Entities;
using Isu.Extra.Services;

namespace Isu.Extra.Exceptions;

public class MegaFacultyAlreadyExistsException : IsuExtraException
{
    public MegaFacultyAlreadyExistsException(MegaFaculty megaFaculty)
        : base($"megafaculty {megaFaculty.Name} already exists") { }
}