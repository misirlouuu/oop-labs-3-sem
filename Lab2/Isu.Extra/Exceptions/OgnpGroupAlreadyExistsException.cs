using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class OgnpGroupAlreadyExistsException : IsuExtraException
{
    public OgnpGroupAlreadyExistsException(OgnpGroup group)
        : base($"ognp group {group.GroupName} already exists") { }
}