using System.Collections.ObjectModel;

namespace Banks.Entities;

public class Client : IEquatable<Client>
{
    private readonly List<string> _notifications = new ();
    public Client(string firstName, string secondName, string? passport, string? address)
    {
        FirstName = firstName;
        SecondName = secondName;
        Passport = passport;
        Address = address;

        Id = Guid.NewGuid();
    }

    public string FirstName { get; }
    public string SecondName { get; }
    public Guid Id { get; }
    public string? Passport { get; private set; }
    public string? Address { get; private set; }

    public bool IsDoubtful() => string.IsNullOrEmpty(Passport) || string.IsNullOrEmpty(Address);

    public void ChangePassport(string passport)
    {
        if (string.IsNullOrWhiteSpace(passport))
            throw new ArgumentNullException(passport);
        Passport = passport;
    }

    public void ChangeAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentNullException(address);
        Address = address;
    }

    public void Update(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(message);
        _notifications.Add(message);
    }

    public bool Equals(Client? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Client)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_notifications, FirstName, SecondName, Id);
    }
}