namespace Backups.Exceptions;

public class BackupException : Exception
{
    public BackupException() { }

    public BackupException(string message)
        : base(message) { }

    public BackupException(string message, Exception inner)
        : base(message, inner) { }
}