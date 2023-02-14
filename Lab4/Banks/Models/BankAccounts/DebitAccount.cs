using System.Globalization;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Models.BankConfigurations;
using Banks.Models.Transactions;

namespace Banks.Models.BankAccounts;

public class DebitAccount : IAccount, IEquatable<DebitAccount>
{
    private readonly List<ITransaction> _transactions = new ();
    private readonly Calendar _calendar = new GregorianCalendar();
    private int _daysCounter;
    private decimal _interest;

    public DebitAccount(Client client, IBankConfiguration bankConfiguration)
    {
        Money = 0;

        ArgumentNullException.ThrowIfNull(client);
        Client = client;

        ArgumentNullException.ThrowIfNull(bankConfiguration);
        InterestRate = bankConfiguration.DebitInterestRate;
        TransactionLimit = bankConfiguration.TransactionLimit;

        CreationDate = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public decimal Money { get; private set; }
    public Client Client { get; }
    public DateTime CreationDate { get; }
    public Guid Id { get; }
    public IReadOnlyCollection<ITransaction> Transactions => _transactions;
    public decimal InterestRate { get; }
    public decimal TransactionLimit { get; }

    public void ReplenishmentOperation(decimal money)
    {
        if (money <= 0)
            throw new Exception();
        Money += money;
    }

    public void WithdrawOperation(decimal money)
    {
        if (money <= 0 || Money < money)
            throw new Exception();

        if (IsLimitExceeded(money))
            throw new TransactionLimitExceededException();

        Money -= money;
    }

    public void CalculateMonthlyInterest()
    {
        _interest += InterestRate / _calendar.GetDaysInYear(CreationDate.Year) * Money;
        _daysCounter++;

        if (_daysCounter == _calendar.GetDaysInMonth(CreationDate.Year, CreationDate.Month))
        {
            Money += _interest;
            _daysCounter = 0;
            _interest = 0;
        }
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

    public bool Equals(DebitAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((DebitAccount)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool IsLimitExceeded(decimal money) => TransactionLimit < money && Client.IsDoubtful();
}