using Banks.Entities;
using Banks.Exceptions;
using Banks.Models.BankConfigurations;
using Banks.Models.Transactions;

namespace Banks.Models.BankAccounts;

public class CreditAccount : IAccount, IEquatable<CreditAccount>
{
    private readonly List<ITransaction> _transactions = new ();

    public CreditAccount(decimal money, Client client, IBankConfiguration bankConfiguration)
    {
        if (money <= 0)
            throw new Exception();
        Money = money;

        ArgumentNullException.ThrowIfNull(client);
        Client = client;

        ArgumentNullException.ThrowIfNull(bankConfiguration);
        CreditLimit = bankConfiguration.CreditLimit;
        TransactionLimit = bankConfiguration.TransactionLimit;
        Commission = bankConfiguration.Commission;

        CreationDate = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public decimal Money { get; private set; }
    public Client Client { get; }
    public DateTime CreationDate { get; }
    public Guid Id { get; }
    public decimal CreditLimit { get; }
    public decimal TransactionLimit { get; }
    public IReadOnlyCollection<ITransaction> Transactions => _transactions;
    public decimal Commission { get; }

    public void ReplenishmentOperation(decimal money)
    {
        if (money <= 0)
            throw new Exception();
        Money += money;
    }

    public void WithdrawOperation(decimal money)
    {
        if (money <= 0)
            throw new Exception();

        if (IsLimitExceeded(money))
            throw new TransactionLimitExceededException();

        if (IsCreditLimitExceeded(money))
            throw new CreditLimitExceededException();

        if (Money - money < 0)
            Money -= Commission;

        Money -= money;
    }

    public ITransaction AddTransaction(ITransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (_transactions.Contains(transaction))
            throw new Exception();
        _transactions.Add(transaction);

        return transaction;
    }

    public void RemoveTransaction(ITransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (!_transactions.Contains(transaction))
            throw new Exception();
        _transactions.Remove(transaction);
    }

    public ITransaction GetTransaction(ITransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        ITransaction? oldTransaction = _transactions.Find(t => t.Equals(transaction));
        if (oldTransaction is null)
            throw new Exception();

        return oldTransaction;
    }

    public void CancelReplenishment(decimal money)
    {
        if (money <= 0)
            throw new Exception();
        Money -= money;
    }

    public void CalculateMonthlyInterest() { }

    public bool Equals(CreditAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((CreditAccount)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool IsLimitExceeded(decimal money) => TransactionLimit < money && Client.IsDoubtful();
    private bool IsCreditLimitExceeded(decimal money) => Math.Abs(Money - money) > CreditLimit;
}