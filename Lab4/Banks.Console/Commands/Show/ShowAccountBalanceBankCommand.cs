using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Show;

public class ShowAccountBalanceBankCommand : IBankCommand
{
    private readonly Bank _bank;
    private readonly IAccount _account;

    public ShowAccountBalanceBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        _bank = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("account id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        _account = _bank.GetAccount(id);
    }

    public void Execute()
    {
        System.Console.Write($"current balance on the account with id" +
                             $"{_account.Id} in bank {_bank.Name}: {_account.Money}");
    }
}