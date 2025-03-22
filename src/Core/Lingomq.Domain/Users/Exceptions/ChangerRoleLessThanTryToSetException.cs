using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Exceptions;

public class ChangerRoleLessThanTryToSetException : DomainException
{
    public override string Message => "You have role weight less than you want to set";
}
