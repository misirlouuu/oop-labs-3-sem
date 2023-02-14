using Banks.Models.BankAccounts;

namespace Banks.Models.Transactions;

public class WithdrawTransaction : ITransaction, IEquatable<WithdrawTransaction>
{
    public WithdrawTransaction(IAccount account, decimal money)
    {
        ArgumentNullException.ThrowIfNull(account);
        Account = account;

        Money = money;
        IsCanceled = false;
        CreationDate = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public DateTime CreationDate { get; }
    public Guid Id { get; }
    public bool IsCanceled { get; private set; }
    public IAccount Account { get; }
    public decimal Money { get; }

    public void Execute() => Account.WithdrawOperation(Money);

    public void Undo()
    {
        if (IsCanceled)
            throw new Exception();
        Account.ReplenishmentOperation(Money);
        IsCanceled = true;
    }

    public bool Equals(WithdrawTransaction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((WithdrawTransaction)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}