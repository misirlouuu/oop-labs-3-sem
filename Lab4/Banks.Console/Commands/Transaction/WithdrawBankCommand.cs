using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankAccounts;
using Banks.Models.Transactions;

namespace Banks.Console.Commands.Transaction;

public class WithdrawBankCommand : IBankCommand
{
    private readonly decimal _money;
    private readonly Bank _bank;
    private readonly IAccount _account;

    public WithdrawBankCommand(IAccount account, Bank bank, decimal money)
    {
        ArgumentNullException.ThrowIfNull(account);
        _account = account;

        ArgumentNullException.ThrowIfNull(bank);
        _bank = bank;

        _money = money;
    }

    public void Execute()
    {
        ITransaction transaction = _bank.WithdrawOperation(_account, _money);
        System.Console.Write($"funds have been withdrawn from the account with id {_account.Id}: \n" +
                             $"current balance: {_account.Money} \n" +
                             $"transaction id: {transaction.Id}");
    }
}