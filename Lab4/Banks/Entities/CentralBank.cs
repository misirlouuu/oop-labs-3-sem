using Banks.Models.BankAccounts;
using Banks.Models.BankConfigurations;
using Banks.Models.Transactions;
using Timer = Banks.Models.Timers.Timer;

namespace Banks.Entities;

public class CentralBank
{
    private static readonly object Lock = new ();
    private static CentralBank? _instance;
    private readonly List<Bank> _banks = new ();

    private CentralBank()
    {
        Timer = new Timer();
    }

    public static CentralBank Instance
    {
        get
        {
            if (_instance is not null)
                return _instance;

            // double check lock для потокобезопаности
            lock (Lock)
            {
                if (_instance is not null) // проверяем, что объект не был инициализирован другим потоком, пока текущий висел на локе
                    return _instance;

                return _instance = new CentralBank();
            }
        }
    }

    public Timer Timer { get; }

    public IReadOnlyCollection<Bank> Banks => _banks;

    public Bank RegisterBank(string? bankName, IBankConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentNullException(bankName);

        var bank = new Bank(configuration, bankName, Timer);
        if (_banks.Contains(bank))
            throw new Exception();
        _banks.Add(bank);

        return bank;
    }

    public Bank GetBank(string? bankName)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentNullException(bankName);

        Bank? bank = _banks.Find(bank => bank.Name.Equals(bankName));
        if (bank is null)
            throw new Exception();
        return bank;
    }

    public ITransaction TransferMoney(Bank bankFrom, IAccount accountFrom, Bank bankTo, IAccount accountTo, decimal money)
    {
        ArgumentNullException.ThrowIfNull(bankFrom);
        ArgumentNullException.ThrowIfNull(accountFrom);

        ArgumentNullException.ThrowIfNull(bankTo);
        ArgumentNullException.ThrowIfNull(accountTo);

        bankFrom.WithdrawOperation(accountFrom, money);
        bankTo.ReplenishmentOperation(accountTo, money);

        var transaction = new TransferTransaction(accountFrom, accountTo, money);
        accountFrom.AddTransaction(transaction);
        accountTo.AddTransaction(transaction);

        return transaction;
    }

    public void CancelTransaction(Bank bank, ITransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(bank);

        if (!_banks.Contains(bank))
            throw new Exception();

        IAccount account = bank.Accounts.First(account => account.Transactions.Contains(transaction));
        ITransaction oldTransaction = account.GetTransaction(transaction);
        oldTransaction.Undo();
    }

    public void CalculateMonthlyInterest()
    {
        _banks.ForEach(bank => bank.CalculateMonthlyInterest());
    }
}