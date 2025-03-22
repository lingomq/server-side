using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Words;

public class WordThematics : ValueObject
{
    private string _category;
    public string Category => _category;
    public static WordThematics General => new("general");
    public static WordThematics Foods => new("foods");
    public static WordThematics Animals => new("animals");
    public static WordThematics Families => new("family");
    public static WordThematics Works => new("work");
    public static WordThematics Naturals => new("naturals");

    public WordThematics(string category)
    {
        _category = category;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Category;
    }
}
