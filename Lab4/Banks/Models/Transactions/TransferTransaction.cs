using Banks.Models.BankAccounts;

namespace Banks.Models.Transactions;

public class TransferTransaction : ITransaction, IEquatable<TransferTransaction>
{
    public TransferTransaction(IAccount accountFrom, IAccount accountTo, decimal money)
    {
        ArgumentNullException.ThrowIfNull(accountFrom);
        AccountFrom = accountFrom;

        ArgumentNullException.ThrowIfNull(accountTo);
        AccountTo = accountTo;

        Money = money;
        IsCanceled = false;
        CreationDate = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public DateTime CreationDate { get; }
    public Guid Id { get; }
    public bool IsCanceled { get; private set; }
    public IAccount AccountFrom { get; }
    public IAccount AccountTo { get; }
    public decimal Money { get; }

    public void Execute()
    {
        AccountFrom.WithdrawOperation(Money);
        AccountTo.ReplenishmentOperation(Money);
    }

    public void Undo()
    {
        if (IsCanceled)
            throw new Exception();

        AccountTo.WithdrawOperation(Money);
        AccountFrom.ReplenishmentOperation(Money);

        IsCanceled = true;
    }

    public bool Equals(TransferTransaction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((TransferTransaction)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}