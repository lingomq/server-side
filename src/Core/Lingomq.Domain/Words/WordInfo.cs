using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Words;

public class WordInfo : EntityBase<Guid>
{
    private List<WordInfo> _translations = new();
    public string Word { get; private set; } = null!;
    public string Transcription { get; set; } = "";
    public string Description { get; set; } = "";
    public virtual IReadOnlyCollection<WordInfo> Translations => _translations;
    public Language Language { get; set; } = null!;
    public WordThematics Thematics { get; set; } = WordThematics.General;

    public WordInfo(
        string word,
        string description,
        string transcription,
        Language language,
        WordThematics thematics
    )
    {
        Word = word;
        Description = description;
        Transcription = transcription;
        Language = language;
        Thematics = thematics;
    }

    protected WordInfo() { }

    public void ChangeThematics(WordThematics thematics) => Thematics = thematics;

    public void AddNoRecursiveTranslation(WordInfo wordInfo)
    {
        _translations.Add(wordInfo);
    }

    public void AddTranslation(WordInfo info)
    {
        foreach (var translation in _translations)
            translation.AddTranslation(info);

        info.AddTranslations(_translations.ToArray());
        info.AddNoRecursiveTranslation(this);

        _translations.Add(info);
    }

    public void AddTranslations(WordInfo[] translations)
    {
        foreach (var translation in translations)
            AddTranslation(translation);
    }

    public void RemoveTranslation(string translation)
    {
        var value = _translations.FirstOrDefault(x => x.Word == translation);

        _translations.Remove(value!);
    }
}
