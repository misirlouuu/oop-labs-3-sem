using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.BankConfigurations;

namespace Banks.Console.Commands.Update;

public class UpdateBankConfigurationBankCommand : IBankCommand
{
    private readonly Bank _bank;
    private readonly IBankConfiguration _configuration;

    public UpdateBankConfigurationBankCommand()
    {
        System.Console.Write("bank name: ");
        string? bankName = System.Console.ReadLine();
        _bank = CentralBank.Instance.GetBank(bankName);

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
        _bank.ChangeBankConfiguration(_configuration);
        System.Console.Write($"configuration in bank {_bank.Name}");
    }
}