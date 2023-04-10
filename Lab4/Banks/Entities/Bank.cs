using Banks.Models.BankAccounts;
using Banks.Models.BankConfigurations;
using Banks.Models.Builders;
using Banks.Models.Observers;
using Banks.Models.Transactions;
using Timer = Banks.Models.Timers.Timer;

namespace Banks.Entities;

public class Bank : IPublisher
{
    private readonly List<Client> _clients = new ();
    private readonly List<ISubscriber> _subscribedClients = new ();
    private readonly List<IAccount> _accounts = new ();
    private readonly Timer _timer;
    private IBankConfiguration _configuration;

    public Bank(IBankConfiguration configuration, string name, Timer timer)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;

        ArgumentNullException.ThrowIfNull(timer);
        _timer = timer;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;

        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }
    public IReadOnlyCollection<IAccount> Accounts => _accounts;

    public void AddClient(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        if (_clients.Contains(client) || _subscribedClients.Contains(client))
            throw new Exception();

        _clients.Add(client);
        _subscribedClients.Add(client);
    }

    public CreditAccount AddCreditAccount(Client client, decimal money)
    {
        ArgumentNullException.ThrowIfNull(client);

        if (!_clients.Contains(client))
            AddClient(client);

        var account = new CreditAccount(money, client, _configuration);
        if (_accounts.Contains(account))
            throw new Exception();
        _accounts.Add(account);

        return account;
    }

    public DebitAccount AddDebitAccount(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        if (!_clients.Contains(client))
            AddClient(client);

        var account = new DebitAccount(client, _configuration);
        if (_accounts.Contains(account))
            throw new Exception();
        _accounts.Add(account);

        return account;
    }

    public DepositAccount AddDepositAccount(Client client, decimal money, TimeSpan term)
    {
        ArgumentNullException.ThrowIfNull(client);

        if (!_clients.Contains(client))
            AddClient(client);

        var account = new DepositAccount(money, client, _configuration, term, _timer);
        if (_accounts.Contains(account))
            throw new Exception();
        _accounts.Add(account);

        return account;
    }

    public ISubscriber Subscribe(ISubscriber subscriber)
    {
       ArgumentNullException.ThrowIfNull(subscriber);

       if (_subscribedClients.Contains(subscriber))
           throw new Exception();
       _subscribedClients.Add(subscriber);

       return subscriber;
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        if (!_subscribedClients.Contains(subscriber))
            throw new Exception();
        _subscribedClients.Remove(subscriber);
    }

    public void Notify(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new Exception();
        _subscribedClients.ForEach(client => client.Update(message));
    }

    public void CalculateMonthlyInterest()
    {
        _accounts.ForEach(account => account.CalculateMonthlyInterest());
    }

    public ITransaction WithdrawOperation(IAccount account, decimal money)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (!_accounts.Contains(account))
            throw new Exception();

        var transaction = new WithdrawTransaction(account, money);
        transaction.Execute();

        account.AddTransaction(transaction);

        return transaction;
    }

    public ITransaction ReplenishmentOperation(IAccount account, decimal money)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (!_accounts.Contains(account))
            throw new Exception();

        var transaction = new ReplenishmentTransaction(account, money);
        transaction.Execute();

        account.AddTransaction(transaction);

        return transaction;
    }

    public IReadOnlyCollection<IAccount> FindAccounts(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        return _accounts.FindAll(account => account.Client.Equals(client));
    }

    public Client GetClient(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Client? client = _clients.Find(client => client.Id.Equals(id));
        if (client is null)
            throw new Exception();

        return client;
    }

    public IAccount GetAccount(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);

        IAccount? account = _accounts.Find(account => account.Id.Equals(id));
        if (account is null)
            throw new Exception();

        return account;
    }

    public void ChangeBankConfiguration(IBankConfiguration bankConfiguration)
    {
        ArgumentNullException.ThrowIfNull(bankConfiguration);
        _configuration = bankConfiguration;
        Notify("bank configuration was changed");
    }

    public ITransaction GetTransaction(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);

        IAccount? account = _accounts.FirstOrDefault(account => account.Transactions.Any(t => t.Id.Equals(id)));
        if (account is null)
            throw new Exception();

        return account.Transactions.First(t => t.Id.Equals(id));
    }
}