using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.Transactions;

namespace Banks.Console.Commands.Cancel;

public class CancelTransactionBankCommand : IBankCommand
{
    private readonly Bank _bank;
    private readonly ITransaction _transaction;

    public CancelTransactionBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        _bank = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("transaction id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        _transaction = _bank.GetTransaction(id);
    }

    public void Execute()
    {
        CentralBank.Instance.CancelTransaction(_bank, _transaction);
        System.Console.Write($"transaction with id {_transaction.Id} was successfully canceled");
    }
}