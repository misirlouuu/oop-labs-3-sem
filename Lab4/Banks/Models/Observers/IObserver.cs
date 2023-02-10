namespace Banks.Models.Observers;

public interface IObserver
{
    void Update(string message);
}