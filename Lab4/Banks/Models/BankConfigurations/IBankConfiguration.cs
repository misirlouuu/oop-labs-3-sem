namespace Banks.Models.BankConfigurations;

public interface IBankConfiguration
{
    decimal DebitInterestRate { get; }
    IReadOnlyCollection<decimal> DepositLimits { get; }
    IReadOnlyCollection<decimal> DepositPercentages { get; }
    decimal CreditLimit { get; }
    decimal Commission { get; }
    decimal TransactionLimit { get; }

    void ChangeDebitInterestRate(decimal debitInterestRate);
    void ChangeDepositLimits(IReadOnlyCollection<decimal> depositLimits);
    void ChangeDepositPercentages(IReadOnlyCollection<decimal> depositPercentages);
    void ChangeCreditLimit(decimal creditLimit);
    void ChangeCommission(decimal commission);
    void ChangeTransactionLimit(decimal transactionLimit);
}