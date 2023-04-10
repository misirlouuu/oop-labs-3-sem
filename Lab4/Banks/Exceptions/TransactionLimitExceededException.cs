namespace Banks.Exceptions;

public class TransactionLimitExceededException : BanksException
{
    public TransactionLimitExceededException()
        : base("account is doubtful: transaction limit was exceeded") { }
}