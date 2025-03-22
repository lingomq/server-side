using MediatR;

namespace LingoMQ.Core.Domain.Common;

public abstract class EntityBase<T> 
{
    private List<INotification> _domainEvents = new();
    public T? Id { get; set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

    public void AddEvent(INotification notification) => _domainEvents.Add(notification);

    public void ClearEvents() => _domainEvents.Clear();
}
