namespace Isu.Extra.Exceptions;

public class InvalidOgnpGroupNameException : IsuExtraException
{
    public InvalidOgnpGroupNameException()
        : base($"first 3 symbols of group name must be letters, other 3 - digits") { }
}