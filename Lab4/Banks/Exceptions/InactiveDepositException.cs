namespace Banks.Exceptions;

public class InactiveDepositException : BanksException
{
    public InactiveDepositException()
        : base($"cannot access money: the term is not up") { }
}