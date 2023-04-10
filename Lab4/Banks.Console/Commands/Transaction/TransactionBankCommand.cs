using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;

namespace Banks.Console.Commands.Transaction;

public class TransactionBankCommand : IBankCommand
{
    private readonly IBankCommand _bankCommand;

    public TransactionBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        Bank bank = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("account id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        IAccount account = bank.GetAccount(id);

        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.Write("choose type of transaction: \n");
        System.Console.ResetColor();

        System.Console.Write("  -replenishment \n" +
                             "  -withdraw \n" +
                             "  -transfer \n");

        string? transactionType = System.Console.ReadLine();
        decimal money;
        switch (transactionType)
        {
            case "-replenishment":
                System.Console.Write("funds to be credited to the bank account: ");
                money = Convert.ToDecimal(System.Console.ReadLine());
                _bankCommand = new ReplenishmentBankCommand(account, bank, money);
                break;
            case "-withdraw":
                System.Console.Write("funds to be credited to the bank account: ");
                money = Convert.ToDecimal(System.Console.ReadLine());
                _bankCommand = new WithdrawBankCommand(account, bank, money);
                break;
            case "-transfer":
                System.Console.Write("funds to be credited to the bank account: ");
                money = Convert.ToDecimal(System.Console.ReadLine());
                _bankCommand = new TransferBankCommand(account, bank, money);
                break;
            default:
                System.Console.WriteLine("invalid type of transaction");
                break;
        }

        if (_bankCommand is null)
            throw new ArgumentNullException(_bankCommand?.ToString());
    }

    public void Execute()
    {
        _bankCommand.Execute();
    }
}