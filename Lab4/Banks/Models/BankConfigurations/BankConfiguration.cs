namespace Banks.Models.BankConfigurations;

public class BankConfiguration
{
    public BankConfiguration(
        decimal debitInterestRate,
        IReadOnlyCollection<decimal> depositLimits,
        IReadOnlyCollection<decimal> depositPercentages,
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

        ArgumentNullException.ThrowIfNull(depositLimits);
        if (depositLimits.Any(limit => limit <= 0))
            throw new Exception();
        DepositLimits = depositLimits;

        ArgumentNullException.ThrowIfNull(depositPercentages);
        if (depositPercentages.Any(percentage => percentage <= 0))
            throw new Exception();
        DepositPercentages = depositPercentages;
    }

    public decimal DebitInterestRate { get; private set; }
    public IReadOnlyCollection<decimal> DepositLimits { get; private set; }
    public IReadOnlyCollection<decimal> DepositPercentages { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal Commission { get; private set; }
    public decimal TransactionLimit { get; private set; }

    public void ChangeDebitInterestRate(decimal debitInterestRate)
    {
        if (debitInterestRate <= 0)
            throw new Exception();
        DebitInterestRate = debitInterestRate;
    }

    public void ChangeDepositLimits(IReadOnlyCollection<decimal> depositLimits)
    {
        ArgumentNullException.ThrowIfNull(depositLimits);

        if (!DepositPercentages.Count.Equals(depositLimits.Count))
            throw new Exception();
        DepositLimits = depositLimits;
    }

    public void ChangeDepositPercentages(IReadOnlyCollection<decimal> depositPercentages)
    {
        ArgumentNullException.ThrowIfNull(depositPercentages);

        if (!DepositLimits.Count.Equals(depositPercentages.Count))
            throw new Exception();
        DepositPercentages = depositPercentages;
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