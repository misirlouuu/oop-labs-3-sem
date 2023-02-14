using Banks.Console.Interfaces;
using Banks.Entities;
using Banks.Models.Builders;

namespace Banks.Console.Commands.Register;

public class RegisterClientBankCommand : IBankCommand
{
    private readonly Client _client;
    private readonly Bank _bank;

    public RegisterClientBankCommand()
    {
        System.Console.Write("bank name: ");
        _bank = CentralBank.Instance.GetBank(System.Console.ReadLine());

        System.Console.Write("name: ");
        string? name = System.Console.ReadLine();

        System.Console.Write("surname: ");
        string? surname = System.Console.ReadLine();

        System.Console.Write("address: ");
        string? address = System.Console.ReadLine();

        System.Console.Write("passport: ");
        string? passport = System.Console.ReadLine();

        _client = new ClientBuilder().WithFirstName(name).WithSecondName(surname).WithAddress(address)
            .WithPassport(passport).Build();
    }

    public void Execute()
    {
        _bank.AddClient(_client);
        System.Console.Write($"client {_client.FirstName} {_client.SecondName} with id {_client.Id} " +
                                 $"was successfully registered in bank {_bank.Name}");
    }
}