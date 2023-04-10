using System.Globalization;
using System.Transactions;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Models.BankConfigurations;
using Banks.Models.Transactions;
using Timer = Banks.Models.Timers.Timer;

namespace Banks.Models.BankAccounts;

public class DepositAccount : IAccount, IEquatable<DepositAccount>
{
    private readonly List<ITransaction> _transactions = new ();
    private readonly Calendar _calendar = new GregorianCalendar();
    private readonly Timer _timer;
    private int _daysCounter;
    private decimal _interest;

    public DepositAccount(decimal money, Client client, IBankConfiguration bankConfiguration, TimeSpan term, Timer timer)
    {
        if (money <= 0)
            throw new Exception();
        Money = money;

        ArgumentNullException.ThrowIfNull(client);
        Client = client;

        ArgumentNullException.ThrowIfNull(bankConfiguration);
        DepositInformation = bankConfiguration.DepositInformation;
        InterestRate = DepositInformation.GetDepositInterestRate(Money);
        TransactionLimit = bankConfiguration.TransactionLimit;

        ArgumentNullException.ThrowIfNull(term);
        Term = term;

        ArgumentNullException.ThrowIfNull(timer);
        _timer = timer;

        CreationDate = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public decimal Money { get; private set; }
    public Client Client { get; }
    public DateTime CreationDate { get; }
    public Guid Id { get; }
    public IReadOnlyCollection<ITransaction> Transactions => _transactions;
    public TimeSpan Term { get; }
    public decimal InterestRate { get; private set; }
    public DepositInformation DepositInformation { get; }
    public decimal TransactionLimit { get; }
    public DateTime CurrentDate => _timer.CurrentDate;

    public void ReplenishmentOperation(decimal money)
    {
        if (money <= 0)
            throw new Exception();
        Money += money;
        InterestRate = DepositInformation.GetDepositInterestRate(Money);
    }

    public void WithdrawOperation(decimal money)
    {
        if (money <= 0 || Money < money)
            throw new Exception();

        if (IsTermUp() != -1)
            throw new InactiveDepositException();

        if (IsLimitExceeded(money))
            throw new TransactionLimitExceededException();

        Money -= money;
        InterestRate = DepositInformation.GetDepositInterestRate(Money);
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

    public void CancelReplenishment(decimal money)
    {
        if (money <= 0)
            throw new Exception();
        Money -= money;
        InterestRate = DepositInformation.GetDepositInterestRate(Money);
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

    public bool Equals(DepositAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((DepositAccount)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool IsLimitExceeded(decimal money) => TransactionLimit < money && Client.IsDoubtful();

    private int IsTermUp() => (CreationDate + Term).CompareTo(CurrentDate); // -1 - earlier than CD, 1 - later than CD
}