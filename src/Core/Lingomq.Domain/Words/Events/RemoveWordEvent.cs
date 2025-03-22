using MediatR;

namespace LingoMQ.Core.Domain.Words.Events;

public class RemoveWordEvent : INotification
{
    public Guid Id { get; set; }

    public RemoveWordEvent(Guid id)
    {
        Id = id;
    }
}
