namespace Banks.Exceptions;

public class BanksException : Exception
{
    public BanksException() { }

    public BanksException(string message)
        : base(message) { }

    public BanksException(string message, Exception inner)
        : base(message, inner) { }
}