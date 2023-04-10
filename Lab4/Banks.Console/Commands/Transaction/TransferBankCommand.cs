using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;
using Banks.Models.Transactions;

namespace Banks.Console.Commands.Transaction;

public class TransferBankCommand : IBankCommand
{
    private readonly decimal _money;
    private readonly Bank _bankFrom;
    private readonly IAccount _accountFrom;
    private readonly Bank _bankTo;
    private readonly IAccount _accountTo;

    public TransferBankCommand(IAccount accountFrom, Bank bankFrom, decimal money)
    {
        ArgumentNullException.ThrowIfNull(accountFrom);
        _accountFrom = accountFrom;

        ArgumentNullException.ThrowIfNull(bankFrom);
        _bankFrom = bankFrom;

        _money = money;

        System.Console.Write("destination bank name: ");
        string? bankName = System.Console.ReadLine();
        _bankTo = CentralBank.Instance.GetBank(bankName);

        System.Console.Write("destination account id: ");
        var id = new Guid(System.Console.ReadLine() ?? string.Empty);
        _accountTo = _bankTo.GetAccount(id);
    }

    public void Execute()
    {
       ITransaction transaction = CentralBank.Instance.TransferMoney(
            _bankFrom,
            _accountFrom,
            _bankTo,
            _accountTo,
            _money);
       System.Console.Write($"funds have been successfully transferred from the bank {_bankFrom.Name} " +
                             $"from the account with id {_accountFrom.Id} " +
                             $"to bank {_bankTo.Name} to account with id {_accountTo.Id} \n" +
                             $"current balance of the first account: {_accountFrom.Money} \n" +
                             $"current balance of the second account: {_accountTo.Money} \n " +
                             $"transaction id: {transaction.Id}");
    }
}