namespace Banks.Models.BankConfigurations;

public class BankConfiguration : IBankConfiguration
{
    public BankConfiguration(
        decimal debitInterestRate,
        DepositInformation depositInformation,
        decimal creditLimit,
        decimal commission,
        decimal transactionLimit)
    {
        if (debitInterestRate <= 0 || creditLimit <= 0 || commission <= 0 || transactionLimit <= 0)
            throw new Exception();
        DebitInterestRate = debitInterestRate;
        CreditLimit = creditLimit;
        Commission = commission;
        TransactionLimit = transactionLimit;

        ArgumentNullException.ThrowIfNull(depositInformation);
        DepositInformation = depositInformation;
    }

    public decimal DebitInterestRate { get; private set; }
    public DepositInformation DepositInformation { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal Commission { get; private set; }
    public decimal TransactionLimit { get; private set; }

    public void ChangeDebitInterestRate(decimal debitInterestRate)
    {
        if (debitInterestRate <= 0)
            throw new Exception();
        DebitInterestRate = debitInterestRate;
    }

    public void ChangeDepositInformation(DepositInformation depositInformation)
    {
        ArgumentNullException.ThrowIfNull(depositInformation);
        DepositInformation = depositInformation;
    }

    public void ChangeCreditLimit(decimal creditLimit)
    {
        if (creditLimit <= 0)
            throw new Exception();
        CreditLimit = creditLimit;
    }

    public void ChangeCommission(decimal commission)
    {
        if (commission <= 0)
            throw new Exception();
        Commission = commission;
    }

    public void ChangeTransactionLimit(decimal transactionLimit)
    {
        if (transactionLimit <= 0)
            throw new Exception();
        TransactionLimit = transactionLimit;
    }
}