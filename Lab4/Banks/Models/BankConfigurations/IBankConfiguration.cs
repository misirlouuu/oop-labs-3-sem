namespace Banks.Models.BankConfigurations;

public interface IBankConfiguration
{
    decimal DebitInterestRate { get; }
    DepositInformation DepositInformation { get; }
    decimal CreditLimit { get; }
    decimal Commission { get; }
    decimal TransactionLimit { get; }

    void ChangeDebitInterestRate(decimal debitInterestRate);
    void ChangeDepositInformation(DepositInformation depositInformation);
    void ChangeCreditLimit(decimal creditLimit);
    void ChangeCommission(decimal commission);
    void ChangeTransactionLimit(decimal transactionLimit);
}