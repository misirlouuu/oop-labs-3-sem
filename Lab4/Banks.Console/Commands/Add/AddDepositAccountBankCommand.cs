using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Add;

public class AddDepositAccountBankCommand : IBankCommand
{
    private readonly Client _client;
    private readonly Bank _bank;
    private readonly decimal _money;
    private readonly TimeSpan _term;

    public AddDepositAccountBankCommand(Bank bank, Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;

        ArgumentNullException.ThrowIfNull(bank);
        _bank = bank;

        System.Console.Write("balance: ");
        _money = Convert.ToDecimal(System.Console.ReadLine());

        System.Console.Write("days to deposit end date: ");
        int days = Convert.ToInt32(System.Console.ReadLine());
        DateTime endDate = DateTime.Now.AddDays(days + 1);
        _term = endDate - DateTime.Now;
    }

    public void Execute()
    {
        DepositAccount depositAccount = _bank.AddDepositAccount(_client, _money, _term);
        System.Console.Write($"deposit account with id {depositAccount.Id}, balance {depositAccount.Money} " +
                                $"and term {depositAccount.Term.Days} days was successfully opened");
    }
}