using Banks.Console.Interfaces;
using Banks.Entities;

namespace Banks.Console.Commands.Add;

public class AddBankAccountBankCommand : IBankCommand
{
    private readonly IBankCommand _bankCommand;

    public AddBankAccountBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        Bank bank = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("client id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        Client client = bank.GetClient(id);

        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.Write("choose type of account: \n");
        System.Console.ResetColor();

        System.Console.Write("  -debit \n" +
                             "  -credit \n" +
                             "  -deposit \n");

        string? accountType = System.Console.ReadLine();
        switch (accountType)
        {
            case "-debit":
                _bankCommand = new AddDebitAccountBankCommand(bank, client);
                break;
            case "-credit":
                _bankCommand = new AddCreditAccountBankCommand(bank, client);
                break;
            case "-deposit":
                _bankCommand = new AddDepositAccountBankCommand(bank, client);
                break;
            default:
                System.Console.WriteLine("invalid type of account");
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