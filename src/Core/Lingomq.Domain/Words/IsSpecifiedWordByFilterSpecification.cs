using System.Linq.Expressions;
using LingoMQ.Core.Domain.Common.Specifications;

namespace LingoMQ.Core.Domain.Words;

public class IsSpecifiedWordByFilterSpecification : Specification<WordInfo>
{
    private WordThematics _thematics = WordThematics.General;
    private Language _language;
    private string _searchedValue;
    private Language? _languageTo;

    public IsSpecifiedWordByFilterSpecification(
        Language language,
        WordThematics thematics,
        string searchedValue,
        Language? languageTo
    )
    {
        _language = language;
        _thematics = thematics;
        _searchedValue = searchedValue;
        _languageTo = languageTo;
    }

    public override Expression<Func<WordInfo, bool>> ToExpression() =>
        x =>
            (
                x.Language.Value == _language.Value
                && x.Language.Code == _language.Code
                && x.Language.SubCode == _language.SubCode
                && x.Word.Contains(_searchedValue)
                && (
                    _languageTo != null
                        ? (
                            x.Translations.Any(w => w.Language.Value == _languageTo.Value)
                            && x.Translations.Any(w => w.Language.Code == _languageTo.Code)
                            && x.Translations.Any(w => w.Language.SubCode == _languageTo.SubCode)
                        )
                        : true
                )
                && x.Word.Contains(_searchedValue)
            ) && (x.Thematics.Category == _thematics.Category);
}
