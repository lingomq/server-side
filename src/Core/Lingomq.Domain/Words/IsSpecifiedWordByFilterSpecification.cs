using System.Linq.Expressions;
using LingoMQ.Core.Domain.Common.Specifications;

namespace LingoMQ.Core.Domain.Words;

public class IsSpecifiedWordByFilterSpecification : Specification<WordInfo>
{
    private WordThematics _thematics = WordThematics.General;
    private Language _language;
    private string _searchedValue;

    public IsSpecifiedWordByFilterSpecification(Language language, WordThematics thematics, string searchedValue)
    {
        _language = language;
        _thematics = thematics;
        _searchedValue = searchedValue;
    }

    public override Expression<Func<WordInfo, bool>> ToExpression() =>
        x =>
            (
                x.Language.Value == _language.Value
                && x.Language.Code == _language.Code
                && x.Language.SubCode == _language.SubCode
                && x.Word.Contains(_searchedValue)
            ) && (x.Thematics.Category == _thematics.Category);
}
