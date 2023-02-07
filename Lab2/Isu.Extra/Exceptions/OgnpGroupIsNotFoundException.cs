using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class OgnpGroupIsNotFoundException : IsuExtraException
{
    public OgnpGroupIsNotFoundException(OgnpGroup group)
        : base($"ognp group {group.GroupName} is not found") { }

    public OgnpGroupIsNotFoundException()
        : base($"ognp group is not found") { }
}