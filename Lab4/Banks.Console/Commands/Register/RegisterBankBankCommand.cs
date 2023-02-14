using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankConfigurations;

namespace Banks.Console.Commands.Register;

public class RegisterBankBankCommand : IBankCommand
{
    private readonly IBankConfiguration _configuration;
    private readonly string? _bankName;

    public RegisterBankBankCommand()
    {
        System.Console.Write("bank name: ");
        _bankName = System.Console.ReadLine();

        System.Console.Write("debit interest rate: ");
        decimal debitInterestRate = Convert.ToDecimal(System.Console.ReadLine());

        System.Console.Write("credit limit: ");
        decimal creditLimit = Convert.ToDecimal(System.Console.ReadLine());

        System.Console.Write("commission: ");
        decimal commission = Convert.ToDecimal(System.Console.ReadLine());

        System.Console.Write("transaction limit: ");
        decimal transactionLimit = Convert.ToDecimal(System.Console.ReadLine());

        System.Console.Write("number of deposit limits: ");
        int n = Convert.ToInt32(System.Console.ReadLine());

        var depositLimits = new List<decimal>();
        System.Console.Write("deposit limit: ");
        for (int i = 0; i < n; ++i)
        {
            depositLimits.Add(Convert.ToDecimal(System.Console.ReadLine()));
        }

        var depositPercentages = new List<decimal>();
        System.Console.Write("deposit interest rate: ");
        for (int i = 0; i <= n; ++i)
        {
            depositPercentages.Add(Convert.ToDecimal(System.Console.ReadLine()));
        }

        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        _configuration = new BankConfiguration(
            debitInterestRate,
            depositInfo,
            creditLimit,
            commission,
            transactionLimit);
    }

    public void Execute()
    {
        Bank bank = CentralBank.Instance.RegisterBank(_bankName, _configuration);
        System.Console.Write($"bank {bank.Name} with id {bank.Id} was successfully registered");
    }
}