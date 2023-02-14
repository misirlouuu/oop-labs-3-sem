using Banks.Entities;
using Banks.Models.Transactions;

namespace Banks.Models.BankAccounts;

public interface IAccount
{
    decimal Money { get; }
    Client Client { get; }
    DateTime CreationDate { get; }
    Guid Id { get; }
    decimal TransactionLimit { get; }
    IReadOnlyCollection<ITransaction> Transactions { get; }

    void ReplenishmentOperation(decimal money);
    void WithdrawOperation(decimal money);
    void CalculateMonthlyInterest();

    void CancelReplenishment(decimal money);

    ITransaction AddTransaction(ITransaction transaction);
    void RemoveTransaction(ITransaction transaction);
    ITransaction GetTransaction(ITransaction transaction);

    bool Equals(object obj);
}