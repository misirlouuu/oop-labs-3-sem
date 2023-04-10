using Banks.Models.BankAccounts;

namespace Banks.Models.Transactions;

public class ReplenishmentTransaction : ITransaction, IEquatable<ReplenishmentTransaction>
{
    public ReplenishmentTransaction(IAccount account, decimal money)
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

    public void Execute() => Account.ReplenishmentOperation(Money);

    public void Undo()
    {
        if (IsCanceled)
            throw new Exception();
        Account.CancelReplenishment(Money);
        IsCanceled = true;
    }

    public bool Equals(ReplenishmentTransaction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ReplenishmentTransaction)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}