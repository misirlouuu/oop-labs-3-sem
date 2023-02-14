using Banks.Entities;

namespace Banks.Models.Timers;

public class Timer
{
    public Timer()
    {
        CurrentDate = DateTime.Now;
    }

    public DateTime CurrentDate { get; private set; }

    public void RewindTime(TimeSpan term)
    {
        ArgumentNullException.ThrowIfNull(term);

        CurrentDate += term;
        for (int i = 0; i < term.TotalDays; ++i)
        {
            CentralBank.Instance.CalculateMonthlyInterest();
        }
    }
}