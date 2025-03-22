using MediatR;

namespace LingoMQ.Core.Domain.Words.Events;

public class AddedWordTranslationEvent : INotification
{
    public WordInfo Word { get; set; }

    public AddedWordTranslationEvent(WordInfo word)
    {
        Word = word;
    }
}
