using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Add;

public class AddDebitAccountBankCommand : IBankCommand
{
    private readonly Client _client;
    private readonly Bank _bank;

    public AddDebitAccountBankCommand(Bank bank, Client client)
    {
        ArgumentNullException.ThrowIfNull(bank);
        _bank = bank;

        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    public void Execute()
    {
        DebitAccount debitAccount = _bank.AddDebitAccount(_client);
        System.Console.Write($"credit account with id {debitAccount.Id} was successfully opened");
    }
}