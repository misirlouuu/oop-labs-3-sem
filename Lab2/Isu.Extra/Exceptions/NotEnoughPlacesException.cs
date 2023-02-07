namespace Isu.Extra.Exceptions;

public class NotEnoughPlacesException : IsuExtraException
{
    public NotEnoughPlacesException()
        : base("not enough places") { }
}