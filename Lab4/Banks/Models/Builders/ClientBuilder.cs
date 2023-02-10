using Banks.Entities;

namespace Banks.Models.Builders;

public class ClientBuilder
{
    private string? _firstName;
    private string? _secondName;
    private string? _passport;
    private string? _address;

    public ClientBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public ClientBuilder WithSecondName(string secondName)
    {
        _secondName = secondName;
        return this;
    }

    public ClientBuilder WithPassport(string passport)
    {
        _passport = passport;
        return this;
    }

    public ClientBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public Client Build()
    {
        return new Client(
            _firstName ?? throw new InvalidOperationException(),
            _secondName ?? throw new InvalidOperationException(),
            _passport,
            _address);
    }
}