using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Show;

public class ShowAccountsBankCommand : IBankCommand
{
    private readonly IReadOnlyCollection<IAccount> _accounts;

    public ShowAccountsBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        Bank bank = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("client id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        Client client = bank.GetClient(id);

        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.Write($"accounts of client {client.FirstName} {client.SecondName} " +
                             $"with id {client.Id} in bank {bank.Name}: \n");
        System.Console.ResetColor();

        _accounts = bank.FindAccounts(client);
    }

    public void Execute()
    {
        foreach (IAccount account in _accounts)
        {
            System.Console.WriteLine($"{account.GetType().Name}, id: {account.Id}");
        }
    }
}