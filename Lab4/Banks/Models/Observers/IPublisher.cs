namespace Banks.Models.Observers;

public interface IPublisher
{
    ISubscriber Subscribe(ISubscriber subscriber);
    void Unsubscribe(ISubscriber subscriber);
    void Notify(string message);
}