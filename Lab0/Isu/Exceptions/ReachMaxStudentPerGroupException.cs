namespace Isu.Exceptions;

public class ReachMaxStudentPerGroupException : Exception
{
    public ReachMaxStudentPerGroupException(string message)
        : base(message) { }
}