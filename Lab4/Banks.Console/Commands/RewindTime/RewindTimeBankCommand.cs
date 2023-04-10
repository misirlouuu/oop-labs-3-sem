using Banks.Console.Interfaces;
using Banks.Entities;

namespace Banks.Console.Commands.RewindTime;

public class RewindTimeBankCommand : IBankCommand
{
    private readonly TimeSpan _term;
    public RewindTimeBankCommand()
    {
        System.Console.Write("days in term: ");
        int days = Convert.ToInt32(System.Console.ReadLine());
        _term = new TimeSpan(days, 0, 0, 0);
    }

    public void Execute()
    {
        CentralBank.Instance.Timer.RewindTime(_term);
        System.Console.Write($"current time: {CentralBank.Instance.Timer.CurrentDate}");
    }
}