namespace Isu.Extra.Exceptions;

public class InvalidCountException : IsuExtraException
{
    public InvalidCountException()
        : base("count must be positive") { }
}