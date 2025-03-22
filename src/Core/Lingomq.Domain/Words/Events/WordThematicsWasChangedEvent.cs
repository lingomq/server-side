using MediatR;

namespace LingoMQ.Core.Domain.Words.Events;

public class WordThematicsWasChangedEvent : INotification
{
    public WordInfo Word { get; set; }

    public WordThematicsWasChangedEvent(WordInfo word) => Word = word;
}
