namespace Isu.Exceptions;

public class InvalidGroupNameException : Exception
{
    public InvalidGroupNameException(string message)
        : base(message) { }
}