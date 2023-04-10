namespace Banks.Exceptions;

public class CreditLimitExceededException : BanksException
{
    public CreditLimitExceededException()
        : base($"credit limit was exceeded") { }
}