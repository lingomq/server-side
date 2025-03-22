using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Exceptions;

public class AuthorizationTypeIsAlreadySetException : DomainException
{
    public override string Message => "Auth type has been set"; 
}
