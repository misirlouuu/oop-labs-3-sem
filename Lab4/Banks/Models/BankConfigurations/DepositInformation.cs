namespace Banks.Models.BankConfigurations;

public class DepositInformation
{
    public DepositInformation(List<decimal> depositLimits, List<decimal> depositPercentages)
    {
        ArgumentNullException.ThrowIfNull(depositLimits);
        if (depositLimits.Any(limit => limit <= 0))
            throw new Exception();
        DepositLimits = depositLimits;

        ArgumentNullException.ThrowIfNull(depositPercentages);
        if (depositPercentages.Any(percentage => percentage <= 0))
            throw new Exception();
        DepositPercentages = depositPercentages;

        if (!(depositLimits.Count + 1).Equals(depositPercentages.Count))
            throw new Exception();
    }

    public List<decimal> DepositLimits { get; private set; }
    public List<decimal> DepositPercentages { get; private set; }

    public void ChangeDepositLimits(List<decimal> depositLimits)
    {
        ArgumentNullException.ThrowIfNull(depositLimits);

        if (!DepositPercentages.Count.Equals(depositLimits.Count))
            throw new Exception();
        DepositLimits = depositLimits;
    }

    public void ChangeDepositPercentages(List<decimal> depositPercentages)
    {
        ArgumentNullException.ThrowIfNull(depositPercentages);

        if (!DepositLimits.Count.Equals(depositPercentages.Count))
            throw new Exception();
        DepositPercentages = depositPercentages;
    }

    public decimal GetDepositInterestRate(decimal money)
    {
        if (money <= 0)
            throw new Exception();

        decimal percentage = DepositPercentages.First();
        for (int i = 0; i < DepositLimits.Count; ++i)
        {
            if (money > DepositLimits[i])
                percentage = DepositPercentages[i + 1];
        }

        return percentage;
    }
}