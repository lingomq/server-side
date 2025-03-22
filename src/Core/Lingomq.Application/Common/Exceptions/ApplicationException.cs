namespace LingoMQ.Core.Application.Common.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string text) : base(text) { }
}