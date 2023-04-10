using Banks.Entities;
using Banks.Exceptions;
using Banks.Models.BankAccounts;
using Banks.Models.BankConfigurations;
using Banks.Models.Builders;
using Banks.Models.Transactions;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    private readonly CentralBank _centralBank = CentralBank.Instance;

    [Fact]
    public void AddBank_CentralBankContainsBank()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("tinkoff", bankConfiguration);

        Assert.Contains(bank, _centralBank.Banks);
    }

    [Fact]
    public void ExceedTransactionLimit_ThrowException()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("sberbank", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").Build();
        DebitAccount debitAccount = bank.AddDebitAccount(client);
        bank.ReplenishmentOperation(debitAccount, 500m);

        Assert.True(client.IsDoubtful());
        Assert.Throws<TransactionLimitExceededException>(() => bank.WithdrawOperation(debitAccount, 400m));
    }

    [Fact]
    public void ExceedCreditLimit_ThrowException()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("alfa bank", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();
        CreditAccount creditAccount = bank.AddCreditAccount(client, 50m);

        Assert.Throws<CreditLimitExceededException>(() => bank.WithdrawOperation(creditAccount, 250m));
    }

    [Fact]
    public void WithdrawMoneyFromInactiveDeposit_ThrowException()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("tinkoff", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();
        DepositAccount depositAccount = bank.AddDepositAccount(client, 70m, new TimeSpan(120, 0, 0, 0));

        Assert.Equal(3.5m, depositAccount.InterestRate);
        Assert.Throws<InactiveDepositException>(() => bank.WithdrawOperation(depositAccount, 50m));
    }

    [Fact]
    public void RewindTime_MonthlyInterestWasCalculatedProperly()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4.5m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("tinkoff", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();
        DebitAccount debitAccount = bank.AddDebitAccount(client);
        bank.ReplenishmentOperation(debitAccount, 70m);

        _centralBank.Timer.RewindTime(new TimeSpan(28, 0, 0, 0));
        Assert.Equal(1.5m, debitAccount.InterestRate);
        Assert.Equal(78m, decimal.Round(debitAccount.Money));
    }

    [Fact]
    public void TransferMoney_TransferWasSuccessful()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bankFrom = _centralBank.RegisterBank("tinkoff", bankConfiguration);
        Bank bankTo = _centralBank.RegisterBank("sberbank", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();

        DebitAccount accountFrom = bankFrom.AddDebitAccount(client);
        DebitAccount accountTo = bankTo.AddDebitAccount(client);

        bankFrom.ReplenishmentOperation(accountFrom, 650m);
        bankTo.ReplenishmentOperation(accountTo, 730m);

        _centralBank.TransferMoney(bankFrom, accountFrom, bankTo, accountTo, 150m);

        Assert.Equal(500m, accountFrom.Money);
        Assert.Equal(880m, accountTo.Money);
    }

    [Fact]
    public void CancelTransaction_FundsHaveBeenReturned()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bankFrom = _centralBank.RegisterBank("tinkoff", bankConfiguration);
        Bank bankTo = _centralBank.RegisterBank("sberbank", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();

        DebitAccount accountFrom = bankFrom.AddDebitAccount(client);
        DebitAccount accountTo = bankTo.AddDebitAccount(client);

        bankFrom.ReplenishmentOperation(accountFrom, 650m);
        bankTo.ReplenishmentOperation(accountTo, 730m);

        ITransaction transaction = _centralBank.TransferMoney(bankFrom, accountFrom, bankTo, accountTo, 150m);
        _centralBank.CancelTransaction(bankFrom, transaction);

        Assert.True(accountFrom.GetTransaction(transaction).IsCanceled);
        Assert.True(accountTo.GetTransaction(transaction).IsCanceled);

        Assert.Equal(650m, accountFrom.Money);
        Assert.Equal(730m, accountTo.Money);
    }

    [Fact]
    public void WithdrawMoneyFromCreditAccount_CommissionWasTaken()
    {
        var depositLimits = new List<decimal> { 50m, 100m, 500m };
        var depositPercentages = new List<decimal> { 3m, 3.5m, 4m, 5m };
        var depositInfo = new DepositInformation(depositLimits, depositPercentages);

        var bankConfiguration = new BankConfiguration(1.5m, depositInfo, 100m, 100m, 100m);
        Bank bank = _centralBank.RegisterBank("alfa bank", bankConfiguration);

        Client client = new ClientBuilder().WithFirstName("name").WithSecondName("surname").WithAddress("address")
            .WithPassport("33344445555").Build();
        CreditAccount creditAccount = bank.AddCreditAccount(client, 50m);
        bank.WithdrawOperation(creditAccount, 70m);

        Assert.Equal(-120m, creditAccount.Money);
    }
}