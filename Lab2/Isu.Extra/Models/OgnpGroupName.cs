using Isu.Exceptions;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public record OgnpGroupName
{
    public OgnpGroupName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);

        Validate(name);
        Name = name;
    }

    public string Name { get; }

    private static void Validate(string name)
    {
        if (name.Length != 6)
            throw new InvalidOgnpGroupNameException();

        if (name[..2].Any(symbol => !char.IsLetter(symbol)))
            throw new InvalidOgnpGroupNameException();

        if (name[3..].Any(symbol => !char.IsDigit(symbol)))
            throw new InvalidOgnpGroupNameException();
    }
}