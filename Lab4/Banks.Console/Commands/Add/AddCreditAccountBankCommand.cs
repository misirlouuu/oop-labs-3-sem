using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Add;

public class AddCreditAccountBankCommand : IBankCommand
{
    private readonly Client _client;
    private readonly Bank _bank;
    private readonly decimal _money;

    public AddCreditAccountBankCommand(Bank bank, Client client)
    {
        ArgumentNullException.ThrowIfNull(bank);
        _bank = bank;

        ArgumentNullException.ThrowIfNull(client);
        _client = client;

        System.Console.Write("balance: ");
        _money = Convert.ToDecimal(System.Console.ReadLine());
    }

    public void Execute()
    {
        CreditAccount creditAccount = _bank.AddCreditAccount(_client, _money);
        System.Console.Write($"credit account with id {creditAccount.Id} and balance {creditAccount.Money} " +
                                 $"was successfully opened");
    }
}