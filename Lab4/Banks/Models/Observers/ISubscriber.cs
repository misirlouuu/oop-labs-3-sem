namespace Banks.Models.Observers;

public interface ISubscriber
{
    void Update(string message);
}